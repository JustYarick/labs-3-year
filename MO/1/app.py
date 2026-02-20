import tkinter as tk
from tkinter import ttk, scrolledtext
import numpy as np
from scipy.optimize import linprog
import matplotlib
matplotlib.use("TkAgg")
import matplotlib.pyplot as plt
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg


class WhiskeyLPApp:
    def __init__(self, root):
        self.root = root
        self.root.title("Решение задачи ЛП — Смеси виски")
        self.root.geometry("1100x800")

        mix_frame = ttk.LabelFrame(root, text="Цена продажи смесей (у.е. за 1 л)")
        mix_frame.pack(fill="x", padx=10, pady=5)

        self.price1 = tk.DoubleVar(value=68)
        self.price2 = tk.DoubleVar(value=57)
        self.price3 = tk.DoubleVar(value=45)

        ttk.Label(mix_frame, text="Старый Джек:").grid(row=0, column=0, padx=8, pady=4)
        ttk.Entry(mix_frame, textvariable=self.price1, width=8).grid(row=0, column=1)
        ttk.Label(mix_frame, text="Специальное:").grid(row=0, column=2, padx=8)
        ttk.Entry(mix_frame, textvariable=self.price2, width=8).grid(row=0, column=3)
        ttk.Label(mix_frame, text="Юный Френзи:").grid(row=0, column=4, padx=8)
        ttk.Entry(mix_frame, textvariable=self.price3, width=8).grid(row=0, column=5)

        raw_frame = ttk.LabelFrame(root, text="Запасы сырья и стоимость (у.е. за 1 л)")
        raw_frame.pack(fill="x", padx=10, pady=5)

        self.irish_vol   = tk.DoubleVar(value=2000)
        self.scotch_vol  = tk.DoubleVar(value=1500)
        self.canadian_vol = tk.DoubleVar(value=1200)

        self.irish_cost   = tk.DoubleVar(value=70)
        self.scotch_cost  = tk.DoubleVar(value=50)
        self.canadian_cost = tk.DoubleVar(value=40)

        headers = ["", "Ирландское", "Шотландское", "Канадское"]
        for col, h in enumerate(headers):
            ttk.Label(raw_frame, text=h, font=("Arial", 9, "bold")).grid(row=0, column=col, padx=10)

        ttk.Label(raw_frame, text="Запас (л):").grid(row=1, column=0, padx=8, pady=2)
        ttk.Entry(raw_frame, textvariable=self.irish_vol,    width=8).grid(row=1, column=1)
        ttk.Entry(raw_frame, textvariable=self.scotch_vol,   width=8).grid(row=1, column=2)
        ttk.Entry(raw_frame, textvariable=self.canadian_vol, width=8).grid(row=1, column=3)

        ttk.Label(raw_frame, text="Стоимость:").grid(row=2, column=0, padx=8, pady=2)
        ttk.Entry(raw_frame, textvariable=self.irish_cost,    width=8).grid(row=2, column=1)
        ttk.Entry(raw_frame, textvariable=self.scotch_cost,   width=8).grid(row=2, column=2)
        ttk.Entry(raw_frame, textvariable=self.canadian_cost, width=8).grid(row=2, column=3)

        ttk.Button(root, text="▶  Решить задачу", command=self.solve).pack(pady=8)

        paned = tk.PanedWindow(root, orient=tk.HORIZONTAL)
        paned.pack(fill="both", expand=True, padx=10, pady=5)

        self.output = scrolledtext.ScrolledText(paned, width=45, font=("Consolas", 10))
        paned.add(self.output)

        self.fig_frame = ttk.Frame(paned)
        paned.add(self.fig_frame)

    def log(self, text=""):
        self.output.insert(tk.END, text + "\n")
        self.output.see(tk.END)

    def solve(self):
        self.output.delete(1.0, tk.END)

        p1 = self.price1.get()
        p2 = self.price2.get()
        p3 = self.price3.get()

        I  = self.irish_vol.get()
        S  = self.scotch_vol.get()
        C  = self.canadian_vol.get()

        ci = self.irish_cost.get()
        cs = self.scotch_cost.get()
        cc = self.canadian_cost.get()

        irish_share    = [0.60, 0.15, 0.00]
        canadian_share = [0.20, 0.60, 0.50]
        scotch_share   = [0.20, 0.25, 0.50]

        cost1 = irish_share[0]*ci + canadian_share[0]*cc + scotch_share[0]*cs
        cost2 = irish_share[1]*ci + canadian_share[1]*cc + scotch_share[1]*cs
        cost3 = irish_share[2]*ci + canadian_share[2]*cc + scotch_share[2]*cs

        m1 = p1 - cost1
        m2 = p2 - cost2
        m3 = p3 - cost3

        self.log("=== ПОСТАНОВКА ЗАДАЧИ ===\n")
        self.log("Переменные:")
        self.log("  x1 — объём «Старый Джек»  (л/день)")
        self.log("  x2 — объём «Специальное»  (л/день)")
        self.log("  x3 — объём «Юный Френзи»  (л/день)\n")

        self.log("Расчёт прибыли на 1 л смеси:")
        self.log(f"  «Старый Джек»:  {p1} − ({0.60}·{ci}+{0.20}·{cc}+{0.20}·{cs}) = {p1} − {cost1:.1f} = {m1:.1f}")
        self.log(f"  «Специальное»:  {p2} − ({0.15}·{ci}+{0.60}·{cc}+{0.25}·{cs}) = {p2} − {cost2:.1f} = {m2:.1f}")
        self.log(f"  «Юный Френзи»:  {p3} − ({0.00}·{ci}+{0.50}·{cc}+{0.50}·{cs}) = {p3} − {cost3:.1f} = {m3:.1f}\n")

        self.log("Целевая функция (максимизация ПРИБЫЛИ):")
        self.log(f"  Z = {m1:.1f}·x1 + {m2:.1f}·x2 + {m3:.1f}·x3 → max\n")

        self.log("Ограничения по запасам сырья:")
        self.log(f"  {irish_share[0]}x1 + {irish_share[1]}x2 + {irish_share[2]}x3 ≤ {I}   (ирландское)")
        self.log(f"  {canadian_share[0]}x1 + {canadian_share[1]}x2 + {canadian_share[2]}x3 ≤ {C}   (канадское)")
        self.log(f"  {scotch_share[0]}x1 + {scotch_share[1]}x2 + {scotch_share[2]}x3 ≤ {S}   (шотландское)")
        self.log("  x1, x2, x3 ≥ 0\n")

        c_lp = [-m1, -m2, -m3]   # минимизируем -прибыль

        A_ub = [
            [irish_share[0],    irish_share[1],    irish_share[2]],
            [canadian_share[0], canadian_share[1], canadian_share[2]],
            [scotch_share[0],   scotch_share[1],   scotch_share[2]],
        ]
        b_ub = [I, C, S]

        res = linprog(c_lp, A_ub=A_ub, b_ub=b_ub, bounds=[(0, None)]*3, method="highs")

        if not res.success:
            self.log("❌ Решение не найдено!")
            return

        x1, x2, x3 = res.x
        Z = m1*x1 + m2*x2 + m3*x3

        revenue = p1*x1 + p2*x2 + p3*x3
        raw_cost = (ci*(irish_share[0]*x1+irish_share[1]*x2+irish_share[2]*x3) +
                    cc*(canadian_share[0]*x1+canadian_share[1]*x2+canadian_share[2]*x3) +
                    cs*(scotch_share[0]*x1+scotch_share[1]*x2+scotch_share[2]*x3))

        self.log("=== РЕЗУЛЬТАТ ===\n")
        self.log(f"  x1 «Старый Джек»  = {x1:.2f} л/день")
        self.log(f"  x2 «Специальное»  = {x2:.2f} л/день")
        self.log(f"  x3 «Юный Френзи»  = {x3:.2f} л/день\n")
        self.log(f"  Выручка           = {revenue:.2f} у.е.")
        self.log(f"  Затраты на сырьё  = {raw_cost:.2f} у.е.")
        self.log(f"  ──────────────────────────")
        self.log(f"  Макс. прибыль Z   = {Z:.2f} у.е./день\n")

        used_i = irish_share[0]*x1 + irish_share[1]*x2 + irish_share[2]*x3
        used_c = canadian_share[0]*x1 + canadian_share[1]*x2 + canadian_share[2]*x3
        used_s = scotch_share[0]*x1 + scotch_share[1]*x2 + scotch_share[2]*x3
        self.log("Использование сырья:")
        self.log(f"  Ирландское:   {used_i:.1f} / {I:.0f} л ({used_i/I*100:.1f}%)")
        self.log(f"  Канадское:    {used_c:.1f} / {C:.0f} л ({used_c/C*100:.1f}%)")
        self.log(f"  Шотландское:  {used_s:.1f} / {S:.0f} л ({used_s/S*100:.1f}%)")

        self.draw_graphs(m1, m2, I, C, S, x1, x2, Z)

    def draw_graphs(self, m1, m2, I, C, S, x1_opt, x2_opt, Z_opt):
        for w in self.fig_frame.winfo_children():
            w.destroy()

        fig, axes = plt.subplots(1, 2, figsize=(8, 4))
        fig.suptitle("Z = {:.1f}x₁ + {:.1f}x₂ → max".format(m1, m2), fontsize=11)

        for ax, title, show_opt in zip(axes,
            ["Рис.1 – Область допустимых решений",
             "Рис.2 – Нахождение оптимума"],
            [False, True]):

            xv = np.linspace(0, 4000, 600)
            yv = np.linspace(0, 4000, 600)
            Xg, Yg = np.meshgrid(xv, yv)

            feasible = (
                (0.60*Xg + 0.15*Yg <= I) &
                (0.20*Xg + 0.60*Yg <= C) &
                (0.20*Xg + 0.25*Yg <= S)
            )
            ax.contourf(Xg, Yg, feasible.astype(float),
                        levels=[0.5, 1], alpha=0.3, colors=["#4a90d9"])

            y1 = (I - 0.60*xv) / 0.15
            y2 = (C - 0.20*xv) / 0.60
            y3 = (S - 0.20*xv) / 0.25

            ax.plot(xv, y1, "b-", lw=1.5, label="(1) Ирл.")
            ax.plot(xv, y2, "r-", lw=1.5, label="(2) Канад.")
            ax.plot(xv, y3, "g-", lw=1.5, label="(3) Шотл.")

            if show_opt:
                for k in [Z_opt*0.35, Z_opt*0.6, Z_opt*0.85]:
                    if m2 != 0:
                        yz = (k - m1*xv) / m2
                        ax.plot(xv, yz, "k--", lw=1, alpha=0.5)
                if m2 != 0:
                    yz_opt = (Z_opt - m1*xv) / m2
                    ax.plot(xv, yz_opt, "k-", lw=2, label=f"Z={Z_opt:.0f}")

                ax.scatter(x1_opt, x2_opt, color="red", s=80, zorder=5)
                ax.text(x1_opt + 100, x2_opt - 300,
                        f"x₁={x1_opt:.0f}\nx₂={x2_opt:.0f}\nZ={Z_opt:.0f}",
                        fontsize=8, color="darkred",
                        bbox=dict(boxstyle="round", facecolor="lightyellow"))

                norm = np.sqrt(m1**2 + m2**2)
                ax.annotate("", xy=(500 + m1/norm*600, 2500 + m2/norm*600),
                            xytext=(500, 2500),
                            arrowprops=dict(arrowstyle="->", color="purple", lw=2))
                ax.text(500 + m1/norm*600 + 50, 2500 + m2/norm*600 + 50,
                        "∇Z", color="purple", fontsize=10)

            ax.set_xlim(0, 4000)
            ax.set_ylim(0, 4000)
            ax.set_xlabel("x₁ (Старый Джек)", fontsize=9)
            ax.set_ylabel("x₂ (Специальное)", fontsize=9)
            ax.set_title(title, fontsize=9)
            ax.legend(fontsize=8)
            ax.grid(True, alpha=0.3)

        plt.tight_layout()

        canvas = FigureCanvasTkAgg(fig, master=self.fig_frame)
        canvas.draw()
        canvas.get_tk_widget().pack(fill="both", expand=True)


if __name__ == "__main__":
    root = tk.Tk()
    app = WhiskeyLPApp(root)
    root.mainloop()
