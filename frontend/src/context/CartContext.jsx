import { createContext, useContext, useEffect, useReducer } from "react";

const CartContext = createContext();

const initialState = {
  items: [],
};

function reducer(state, action) {
  switch (action.type) {
    case "LOAD":
      return { items: action.payload };
    case "ADD": {
      const existing = state.items.find((i) => i.id === action.payload.id);
      if (existing) {
        return {
          items: state.items.map((i) =>
            i.id === action.payload.id ? { ...i, qty: i.qty + 1 } : i
          ),
        };
      }
      return { items: [...state.items, { ...action.payload, qty: 1 }] };
    }
    case "REMOVE":
      return { items: state.items.filter((i) => i.id !== action.payload) };
    case "QTY":
      return {
        items: state.items.map((i) =>
          i.id === action.payload.id
            ? { ...i, qty: Math.max(1, action.payload.qty) }
            : i
        ),
      };
    case "CLEAR":
      return { items: [] };
    default:
      return state;
  }
}

export function CartProvider({ children }) {
  const [state, dispatch] = useReducer(reducer, initialState);

  useEffect(() => {
    const saved = localStorage.getItem("cart");
    if (saved) {
      dispatch({ type: "LOAD", payload: JSON.parse(saved) });
    }
  }, []);

  useEffect(() => {
    localStorage.setItem("cart", JSON.stringify(state.items));
  }, [state.items]);

  const add = (item) => dispatch({ type: "ADD", payload: item });
  const remove = (id) => dispatch({ type: "REMOVE", payload: id });
  const setQty = (id, qty) => dispatch({ type: "QTY", payload: { id, qty } });
  const clear = () => dispatch({ type: "CLEAR" });

  return (
    <CartContext.Provider value={{ items: state.items, add, remove, setQty, clear }}>
      {children}
    </CartContext.Provider>
  );
}

export function useCart() {
  return useContext(CartContext);
}
