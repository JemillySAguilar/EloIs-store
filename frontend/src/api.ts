import type { AuthResponse, Cart, Category, Order, Product } from "./types";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5000";

type JsonBody = Record<string, unknown>;

async function request<T>(path: string, options: RequestInit = {}): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    headers: {
      "Content-Type": "application/json",
      ...options.headers,
    },
    ...options,
  });

  if (!response.ok) {
    const message = await response.text();
    throw new Error(message || "Nao foi possivel concluir a solicitacao.");
  }

  return response.json() as Promise<T>;
}

function post<T>(path: string, body: JsonBody): Promise<T> {
  return request<T>(path, {
    method: "POST",
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
  getCart: (userId: string) => request<Cart>(`/api/users/${userId}/cart`),
  addCartItem: (userId: string, productId: string, productVariantId: string, quantity: number) =>
    post<Cart>(`/api/users/${userId}/cart/items`, { productId, productVariantId, quantity }),
  checkout: (userId: string, paymentMethod: string) =>
    post<Order>("/api/orders/checkout", { userId, paymentMethod }),
  listOrders: (userId: string) => request<Order[]>(`/api/orders/user/${userId}`),
};
