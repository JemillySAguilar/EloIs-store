export type Category = {
  id: string;
  name: string;
  slug: string;
};

export type ProductVariant = {
  id: string;
  productId: string;
  sku: string;
  size: string;
  color: string;
  stockQuantity: number;
};

export type Product = {
  id: string;
  categoryId: string;
  name: string;
  description: string;
  price: number;
  isActive: boolean;
  imageUrl?: string;
  variants: ProductVariant[];
};

export type AuthResponse = {
  userId: string;
  name: string;
  email: string;
  role: string;
  accessToken: string;
};

export type CartItem = {
  id: string;
  cartId: string;
  productId: string;
  productVariantId: string;
  productName: string;
  unitPrice: number;
  quantity: number;
};

export type Cart = {
  id: string;
  userId: string;
  items: CartItem[];
  updatedAt: string;
};

export type OrderItem = {
  id: string;
  orderId: string;
  productId: string;
  productVariantId: string;
  productName: string;
  unitPrice: number;
  quantity: number;
};

export type Order = {
  id: string;
  userId: string;
  status: "Pending" | "Confirmed" | "Cancelled" | number;
  totalAmount: number;
  items: OrderItem[];
  createdAt: string;
};
