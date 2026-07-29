export function formatCurrency(value: number): string {
  return new Intl.NumberFormat("pt-BR", {
    style: "currency",
    currency: "BRL",
  }).format(value);
}

export function getOrderStatus(status: string | number): string {
  if (status === "Confirmed" || status === 1) return "Confirmado";
  if (status === "Cancelled" || status === 2) return "Cancelado";
  return "Pendente";
}
