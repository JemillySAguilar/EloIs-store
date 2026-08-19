import {
  Heart,
  LogIn,
  LogOut,
  Minus,
  PackageCheck,
  Search,
  ShoppingBag,
  Sparkles,
  UserPlus,
} from "lucide-react";
import { FormEvent, useEffect, useMemo, useState } from "react";
import { api } from "./api";
import type { AuthResponse, Cart, Category, Order, Product, ProductVariant } from "./types";
import { formatCurrency, getOrderStatus } from "./utils";

const SESSION_KEY = "elois-store-session";

type View = "shop" | "product" | "auth" | "cart" | "orders";
type AuthMode = "login" | "register";

function loadSession(): AuthResponse | null {
  const stored = localStorage.getItem(SESSION_KEY);
  return stored ? (JSON.parse(stored) as AuthResponse) : null;
}

export function App() {
  const [products, setProducts] = useState<Product[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [cart, setCart] = useState<Cart | null>(null);
  const [orders, setOrders] = useState<Order[]>([]);
  const [session, setSession] = useState<AuthResponse | null>(() => loadSession());
  const [view, setView] = useState<View>(() => loadSession() ? "shop" : "auth");
  const [authMode, setAuthMode] = useState<AuthMode>("login");
  const [selectedProductId, setSelectedProductId] = useState<string | null>(null);
  const [categoryId, setCategoryId] = useState("all");
  const [searchTerm, setSearchTerm] = useState("");
  const [isLoading, setIsLoading] = useState(true);
  const [message, setMessage] = useState("");

  // Todas as telas da loja exigem uma sessao ativa. Mesmo que alguma acao
  // tente abrir outra tela, visitantes continuam no login.
  const activeView: View = session ? view : "auth";

  const selectedProduct = products.find((product) => product.id === selectedProductId) ?? products[0];

  const filteredProducts = useMemo(() => {
    return products.filter((product) => categoryId === "all" || product.categoryId === categoryId);
  }, [categoryId, products]);

  const cartTotal = useMemo(() => {
    return cart?.items.reduce((sum, item) => sum + item.unitPrice * item.quantity, 0) ?? 0;
  }, [cart]);

  useEffect(() => {
    async function boot() {
      try {
        const [productList, categoryList] = await Promise.all([api.listProducts(), api.listCategories()]);
        setProducts(productList);
        setCategories(categoryList);
      } catch {
        setMessage("Nao foi possivel carregar a loja. Confira se a API esta rodando.");
      } finally {
        setIsLoading(false);
      }
    }

    boot();
  }, []);

  useEffect(() => {
    if (!session) return;
    api.getCart(session.userId, session.accessToken).then(setCart).catch(() => setCart(null));
    api.listOrders(session.userId, session.accessToken).then(setOrders).catch(() => setOrders([]));
  }, [session]);

  async function handleSearch(event: FormEvent) {
    event.preventDefault();
    setIsLoading(true);
    try {
      const results = searchTerm.trim()
        ? await api.searchProducts(searchTerm.trim())
        : await api.listProducts();
      setProducts(results);
      setCategoryId("all");
    } catch {
      setMessage("Busca indisponivel no momento.");
    } finally {
      setIsLoading(false);
    }
  }

  function openProduct(product: Product) {
    setSelectedProductId(product.id);
    setView("product");
    window.scrollTo({ top: 0, behavior: "smooth" });
  }

  async function addToCart(product: Product, variant: ProductVariant, quantity: number) {
    if (!session) {
      setAuthMode("login");
      setView("auth");
      setMessage("Entre ou crie uma conta para montar sua sacola.");
      return;
    }

    try {
      const updatedCart = await api.addCartItem(session.userId, product.id, variant.id, quantity, session.accessToken);
      setCart(updatedCart);
      setMessage("Peca adicionada a sacola.");
      setView("cart");
    } catch {
      setMessage("Nao foi possivel adicionar a peca.");
    }
  }

  async function submitAuth(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    const data = new FormData(event.currentTarget);
    const email = String(data.get("email"));
    const password = String(data.get("password"));
    const name = String(data.get("name") ?? "");

    try {
      const auth =
        authMode === "login"
          ? await api.login(email, password)
          : await api.register(name, email, password);
      localStorage.setItem(SESSION_KEY, JSON.stringify(auth));
      setSession(auth);
      setMessage(`Bem-vinda, ${auth.name}.`);
      setView("shop");
    } catch {
      setMessage("Nao foi possivel autenticar. Confira os dados e tente novamente.");
    }
  }

  async function checkout() {
    if (!session) return;
    try {
      const order = await api.checkout(session.userId, "credit_card", session.accessToken);
      setOrders((current) => [order, ...current]);
      setCart(await api.getCart(session.userId, session.accessToken));
      setMessage("Pedido finalizado com sucesso.");
      setView("orders");
    } catch {
      setMessage("Nao foi possivel finalizar. Verifique se a sacola tem itens.");
    }
  }

  async function logout() {
    if (!session) return;

    try {
      await api.logout(session.email, session.accessToken);
    } catch {
      // A sessao local deve ser encerrada mesmo se a API estiver indisponivel.
    }

    localStorage.removeItem(SESSION_KEY);
    setSession(null);
    setCart(null);
    setOrders([]);
    setMessage("");
    setAuthMode("login");
    setView("auth");
  }

  return (
    <div className="app-shell">
      <header className="topbar">
        <button className="brand" onClick={() => setView("shop")} aria-label="Ir para vitrine">
          <span className="brand-mark">E</span>
          <span>
            <strong>Elois Store</strong>
            <small>Moda feminina</small>
          </span>
        </button>

        <form className="search-box" onSubmit={handleSearch}>
          <Search size={18} />
          <input
            aria-label="Buscar produtos"
            placeholder="Buscar vestido, blusa, cor..."
            value={searchTerm}
            onChange={(event) => setSearchTerm(event.target.value)}
          />
        </form>

        <nav className="nav-actions" aria-label="Navegacao principal">
          <button onClick={() => setView("orders")} disabled={!session} title="Meus pedidos">
            <PackageCheck size={18} />
            <span>Pedidos</span>
          </button>
          <button onClick={() => setView("cart")} title="Sacola">
            <ShoppingBag size={18} />
            <span>Sacola {cart?.items.length ? `(${cart.items.length})` : ""}</span>
          </button>
          {session ? (
            <button onClick={logout} title="Sair">
              <LogOut size={18} />
              <span>Sair</span>
            </button>
          ) : (
            <button onClick={() => setView("auth")} title="Entrar">
              <LogIn size={18} />
              <span>Entrar</span>
            </button>
          )}
        </nav>
      </header>

      {message && (
        <button className="toast" onClick={() => setMessage("")}>
          {message}
        </button>
      )}

      {activeView === "shop" && (
        <main>
          <section className="hero">
            <div className="hero-copy">
              <span className="eyebrow">
                <Sparkles size={16} />
                Colecao selecionada
              </span>
              <h1>Elois Store</h1>
              <p>
                Pecas femininas leves, versateis e cuidadas para sair do basico sem
                perder conforto.
              </p>
              <div className="hero-actions">
                <button onClick={() => document.getElementById("catalog")?.scrollIntoView({ behavior: "smooth" })}>
                  Ver pecas
                </button>
                <button className="ghost" onClick={() => setView(session ? "orders" : "auth")}>
                  {session ? "Meus pedidos" : "Criar conta"}
                </button>
              </div>
            </div>
            <div className="hero-look" aria-hidden="true">
              <img
                src="https://images.unsplash.com/photo-1485968579580-b6d095142e6e?auto=format&fit=crop&w=900&q=80"
                alt=""
              />
            </div>
          </section>

          <section className="catalog-section" id="catalog">
            <div className="section-heading">
              <div>
                <span className="eyebrow">Vitrine</span>
                <h2>Escolhas da semana</h2>
              </div>
              <div className="category-tabs">
                <button className={categoryId === "all" ? "active" : ""} onClick={() => setCategoryId("all")}>
                  Todas
                </button>
                {categories.map((category) => (
                  <button
                    key={category.id}
                    className={categoryId === category.id ? "active" : ""}
                    onClick={() => setCategoryId(category.id)}
                  >
                    {category.name}
                  </button>
                ))}
              </div>
            </div>

            {isLoading ? (
              <p className="empty-state">Carregando pecas...</p>
            ) : filteredProducts.length ? (
              <div className="product-grid">
                {filteredProducts.map((product) => (
                  <ProductCard key={product.id} product={product} onOpen={openProduct} />
                ))}
              </div>
            ) : (
              <p className="empty-state">Nenhuma peca encontrada.</p>
            )}
          </section>
        </main>
      )}

      {activeView === "product" && selectedProduct && (
        <ProductDetail product={selectedProduct} onBack={() => setView("shop")} onAdd={addToCart} />
      )}

      {activeView === "auth" && (
        <main className="center-page">
          <section className="auth-panel">
            <span className="eyebrow">{authMode === "login" ? "Bem-vinda de volta" : "Primeiro acesso"}</span>
            <h2>{authMode === "login" ? "Entrar na sua conta" : "Criar sua conta"}</h2>
            <form onSubmit={submitAuth} className="stack-form">
              {authMode === "register" && <input name="name" placeholder="Nome" required />}
              <input name="email" type="email" placeholder="E-mail" required />
              <input name="password" type="password" placeholder="Senha" required minLength={6} />
              <button type="submit">
                {authMode === "login" ? <LogIn size={18} /> : <UserPlus size={18} />}
                {authMode === "login" ? "Entrar" : "Cadastrar"}
              </button>
            </form>
            <button className="text-button" onClick={() => setAuthMode(authMode === "login" ? "register" : "login")}>
              {authMode === "login" ? "Ainda nao tenho conta" : "Ja tenho conta"}
            </button>
          </section>
        </main>
      )}

      {activeView === "cart" && (
        <main className="split-page">
          <section>
            <span className="eyebrow">Sacola</span>
            <h2>Suas escolhas</h2>
            {!session ? (
              <EmptyAction text="Entre para ver sua sacola." action="Entrar" onClick={() => setView("auth")} />
            ) : cart?.items.length ? (
              <div className="line-list">
                {cart.items.map((item) => (
                  <article className="line-item" key={item.id}>
                    <div>
                      <h3>{item.productName}</h3>
                      <p>{item.quantity} unidade(s)</p>
                    </div>
                    <strong>{formatCurrency(item.unitPrice * item.quantity)}</strong>
                  </article>
                ))}
              </div>
            ) : (
              <EmptyAction text="Sua sacola ainda esta vazia." action="Ver pecas" onClick={() => setView("shop")} />
            )}
          </section>
          <aside className="summary">
            <span>Total</span>
            <strong>{formatCurrency(cartTotal)}</strong>
            <button onClick={checkout} disabled={!cart?.items.length}>
              <ShoppingBag size={18} />
              Finalizar pedido
            </button>
          </aside>
        </main>
      )}

      {activeView === "orders" && (
        <main className="orders-page">
          <span className="eyebrow">Pedidos</span>
          <h2>Historico da conta</h2>
          {!session ? (
            <EmptyAction text="Entre para acompanhar seus pedidos." action="Entrar" onClick={() => setView("auth")} />
          ) : orders.length ? (
            <div className="line-list">
              {orders.map((order) => (
                <article className="order-card" key={order.id}>
                  <div>
                    <h3>Pedido {order.id.slice(0, 8)}</h3>
                    <p>{new Date(order.createdAt).toLocaleDateString("pt-BR")} · {getOrderStatus(order.status)}</p>
                  </div>
                  <strong>{formatCurrency(order.totalAmount)}</strong>
                </article>
              ))}
            </div>
          ) : (
            <EmptyAction text="Voce ainda nao tem pedidos." action="Comprar agora" onClick={() => setView("shop")} />
          )}
        </main>
      )}
    </div>
  );
}

function ProductCard({ product, onOpen }: { product: Product; onOpen: (product: Product) => void }) {
  const variantLabel = product.variants.map((variant) => variant.size).join(" / ");

  return (
    <article className="product-card">
      <button onClick={() => onOpen(product)} className="product-image">
        <img src={product.imageUrl ?? fallbackImage(product.name)} alt={product.name} />
      </button>
      <div className="product-info">
        <span>{variantLabel || "Peca unica"}</span>
        <h3>{product.name}</h3>
        <p>{product.description}</p>
        <div className="product-footer">
          <strong>{formatCurrency(product.price)}</strong>
          <button onClick={() => onOpen(product)} aria-label={`Ver ${product.name}`}>
            <Heart size={18} />
          </button>
        </div>
      </div>
    </article>
  );
}

function ProductDetail({
  product,
  onBack,
  onAdd,
}: {
  product: Product;
  onBack: () => void;
  onAdd: (product: Product, variant: ProductVariant, quantity: number) => void;
}) {
  const [variantId, setVariantId] = useState(product.variants[0]?.id ?? "");
  const [quantity, setQuantity] = useState(1);
  const variant = product.variants.find((item) => item.id === variantId) ?? product.variants[0];

  useEffect(() => {
    setVariantId(product.variants[0]?.id ?? "");
    setQuantity(1);
  }, [product]);

  return (
    <main className="product-detail">
      <button className="text-button" onClick={onBack}>Voltar para vitrine</button>
      <section className="detail-layout">
        <div className="detail-image">
          <img src={product.imageUrl ?? fallbackImage(product.name)} alt={product.name} />
        </div>
        <div className="detail-copy">
          <span className="eyebrow">Pronta entrega</span>
          <h2>{product.name}</h2>
          <p>{product.description}</p>
          <strong className="detail-price">{formatCurrency(product.price)}</strong>

          <label>
            Tamanho e cor
            <select value={variantId} onChange={(event) => setVariantId(event.target.value)}>
              {product.variants.map((item) => (
                <option value={item.id} key={item.id}>
                  {item.size} · {item.color} · {item.stockQuantity} em estoque
                </option>
              ))}
            </select>
          </label>

          <label>
            Quantidade
            <div className="stepper">
              <button onClick={() => setQuantity(Math.max(1, quantity - 1))} aria-label="Diminuir quantidade">
                <Minus size={16} />
              </button>
              <input value={quantity} onChange={(event) => setQuantity(Number(event.target.value) || 1)} />
              <button onClick={() => setQuantity(quantity + 1)} aria-label="Aumentar quantidade">+</button>
            </div>
          </label>

          <button className="primary-wide" disabled={!variant} onClick={() => variant && onAdd(product, variant, quantity)}>
            <ShoppingBag size={18} />
            Adicionar a sacola
          </button>
        </div>
      </section>
    </main>
  );
}

function EmptyAction({ text, action, onClick }: { text: string; action: string; onClick: () => void }) {
  return (
    <div className="empty-action">
      <p>{text}</p>
      <button onClick={onClick}>{action}</button>
    </div>
  );
}

function fallbackImage(name: string): string {
  return name.toLowerCase().includes("blusa")
    ? "https://images.unsplash.com/photo-1554568218-0f1715e72254?auto=format&fit=crop&w=900&q=80"
    : "https://images.unsplash.com/photo-1496747611176-843222e1e57c?auto=format&fit=crop&w=900&q=80";
}
