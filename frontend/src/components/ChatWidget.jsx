import { useState } from "react";

const canned = [
  "Hola, soy la asistente IA de Sfarma. ¿Buscas un producto o ayuda con el ERP?",
  "Puedo guiarte en compras mayoristas, stock y vencimientos.",
  "Para activaciones ERP o demos, deja tu correo y horario.",
];

export default function ChatWidget() {
  const [open, setOpen] = useState(false);
  const [messages, setMessages] = useState([{ from: "bot", text: canned[0] }]);
  const [input, setInput] = useState("");

  const send = () => {
    if (!input.trim()) return;
    const userMsg = { from: "user", text: input.trim() };
    const botMsg = { from: "bot", text: canned[(messages.length + 1) % canned.length] };
    setMessages((prev) => [...prev, userMsg, botMsg]);
    setInput("");
  };

  return (
    <div className="fixed bottom-4 right-4 z-40">
      {open && (
        <div className="w-80 bg-white shadow-xl rounded-2xl border border-gray-200 overflow-hidden mb-3">
          <div className="bg-blue-600 text-white px-4 py-3 flex justify-between items-center">
            <div>
              <div className="font-semibold">Asistente IA Sfarma</div>
              <div className="text-xs text-blue-100">Soporte rápido y guías</div>
            </div>
            <button onClick={() => setOpen(false)}>✕</button>
          </div>
          <div className="h-60 overflow-y-auto p-3 space-y-2 text-sm">
            {messages.map((m, idx) => (
              <div
                key={idx}
                className={`px-3 py-2 rounded-lg ${m.from === "bot" ? "bg-blue-50 text-blue-900" : "bg-gray-100 text-gray-800"} `}
              >
                {m.text}
              </div>
            ))}
          </div>
          <div className="p-3 border-t border-gray-200 flex gap-2">
            <input
              className="flex-1 border rounded px-2 py-1 text-sm"
              placeholder="Escribe tu pregunta"
              value={input}
              onChange={(e) => setInput(e.target.value)}
              onKeyDown={(e) => e.key === "Enter" && send()}
            />
            <button onClick={send} className="px-3 py-1 bg-blue-600 text-white rounded text-sm">
              Enviar
            </button>
          </div>
        </div>
      )}
      <button
        onClick={() => setOpen((o) => !o)}
        className="bg-blue-600 text-white rounded-full shadow-lg px-4 py-3 text-sm font-semibold hover:bg-blue-700"
      >
        {open ? "Cerrar chat" : "Chat IA"}
      </button>
    </div>
  );
}
