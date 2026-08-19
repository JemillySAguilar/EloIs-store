import type { AuthResponse, Cart, Category, Order, Product } from "./types";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5001";

type JsonBody = Record<string, unknown>;

async function request<T>(path: string, options: RequestInit = {}): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...options.headers,
    },
  });

  if (!response.ok) {
    const message = await response.text();
    throw new Error(message || "Nao foi possivel concluir a solicitacao.");
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return response.json() as Promise<T>;
}

function bearer(accessToken: string): HeadersInit {
  return { Authorization: `Bearer ${accessToken}` };
}

function post<T>(path: string, body: JsonBody, accessToken?: string): Promise<T> {
  return request<T>(path, {
    method: "POST",
    headers: accessToken ? bearer(accessToken) : undefined,
    body: JSON.stringify(body),
  });
}

export const api = {
  listProducts: () => request<Product[]>("/api/products"),
  searchProducts: (term: string) =>
    request<Product[]>(`/api/products/search?term=${encodeURIComponent(term)}`),
  getProduct: (id: string) => request<Product>(`/api/products/${id}`),
  listCategories: () => request<Category[]>("/api/categories"),
  login: (email: string, password: string) =>
    post<AuthResponse>("/api/auth/login", { email, password }),
  register: (name: string, email: string, password: string) =>
    post<AuthResponse>("/api/auth/register", { name, email, password }),
  logout: (email: string, accessToken: string) =>
    post<void>("/api/auth/logout", { email }, accessToken),
  getCart: (userId: string, accessToken: string) =>
    request<Cart>(`/api/users/${userId}/cart`, { headers: bearer(accessToken) }),
  addCartItem: (
    userId: string,
    productId: string,
    productVariantId: string,
    quantity: number,
    accessToken: string,
  ) =>
    post<Cart>(
      `/api/users/${userId}/cart/items`,
      { productId, productVariantId, quantity },
      accessToken,
    ),
  checkout: (userId: string, paymentMethod: string, accessToken: string) =>
    post<Order>("/api/orders/checkout", { userId, paymentMethod }, accessToken),
  listOrders: (userId: string, accessToken: string) =>
    request<Order[]>(`/api/orders/user/${userId}`, { headers: bearer(accessToken) }),
};