import numpy as np
import matplotlib.pyplot as plt
from scipy.optimize import linprog

# -----------------------------
# МОДЕЛЬ
# -----------------------------
# Ограничения:
# 0.6x + 0.15y ≤ 2000
# 0.2x + 0.6y  ≤ 1200
# 0.2x + 0.25y ≤ 1500
# x ≥ 0, y ≥ 0
#
# Целевая функция (ПРИБЫЛЬ):
# Z = 8x + 10y → max
# (маржа: Джек = 68-60 = 8, Специальное = 57-47 = 10)
# -----------------------------

A = np.array([[0.6, 0.15], [0.2, 0.6], [0.2, 0.25], [-1, 0], [0, -1]])
b = np.array([2000, 1200, 1500, 0, 0])

# Сетка
x = np.linspace(0, 4000, 800)
y = np.linspace(0, 4000, 800)
X, Y = np.meshgrid(x, y)

# Допустимая область
feasible = (
    (0.6 * X + 0.15 * Y <= 2000)
    & (0.2 * X + 0.6 * Y <= 1200)
    & (0.2 * X + 0.25 * Y <= 1500)
)

# Прямые ограничений
y1 = (2000 - 0.6 * x) / 0.15
y2 = (1200 - 0.2 * x) / 0.6
y3 = (1500 - 0.2 * x) / 0.25

# -----------------------------
# ПОИСК ОПТИМУМА
# -----------------------------
c_lp = [-8, -10]  # минимизируем -Z
res = linprog(c_lp, A_ub=A, b_ub=b, method="highs")
x_opt, y_opt = res.x
z_opt = 8 * x_opt + 10 * y_opt

# -----------------------------
# ГРАФИК 1: Область решений
# -----------------------------
plt.figure(figsize=(10, 8))

plt.contourf(X, Y, feasible, levels=[0.5, 1], alpha=0.3, colors=["#4a90d9"])
plt.plot(x, y1, label="(1) 0.6x₁ + 0.15x₂ = 2000", color="blue")
plt.plot(x, y2, label="(2) 0.2x₁ + 0.6x₂ = 1200", color="red")
plt.plot(x, y3, label="(3) 0.2x₁ + 0.25x₂ = 1500", color="green")

plt.axhline(0, color="black", linewidth=0.8)
plt.axvline(0, color="black", linewidth=0.8)
plt.xlim(0, 4000)
plt.ylim(0, 4000)
plt.xlabel("x₁ (Старый Джек), л/день", fontsize=12)
plt.ylabel("x₂ (Специальное), л/день", fontsize=12)
plt.title("Рисунок 1 – Пространство допустимых решений", fontsize=13)
plt.legend(fontsize=10)
plt.grid(True, alpha=0.4)
plt.tight_layout()
plt.savefig("graph_area.png", dpi=300, bbox_inches="tight")
plt.close()

# -----------------------------
# ГРАФИК 2: Нахождение оптимума
# -----------------------------
plt.figure(figsize=(10, 8))

plt.contourf(X, Y, feasible, levels=[0.5, 1], alpha=0.25, colors=["#4a90d9"])
plt.plot(x, y1, label="(1) 0.6x₁ + 0.15x₂ = 2000", color="blue")
plt.plot(x, y2, label="(2) 0.2x₁ + 0.6x₂ = 1200", color="red")
plt.plot(x, y3, label="(3) 0.2x₁ + 0.25x₂ = 1500", color="green")

# Линии уровня Z = 8x + 10y (штриховые)
for k in [z_opt * 0.3, z_opt * 0.55, z_opt * 0.8]:
    y_z = (k - 8 * x) / 10
    plt.plot(x, y_z, "k--", linewidth=1, alpha=0.6)
    # подпись уровня
    idx = np.argmin(np.abs(x - 500))
    if 0 < y_z[idx] < 4000:
        plt.text(500, y_z[idx] + 80, f"z={k:.0f}", fontsize=9, color="gray")

# Линия оптимального уровня (сплошная)
y_z_opt = (z_opt - 8 * x) / 10
plt.plot(x, y_z_opt, "k-", linewidth=2, label=f"z = {z_opt:.0f} (оптимум)")

# Оптимальная точка
plt.scatter(x_opt, y_opt, color="red", s=120, zorder=5)
plt.text(
    x_opt + 80,
    y_opt - 300,
    f"Оптимум\nx₁ = {x_opt:.1f}\nx₂ = {y_opt:.1f}\nZ = {z_opt:.1f} у.е.",
    fontsize=11,
    color="darkred",
    bbox=dict(boxstyle="round,pad=0.4", facecolor="lightyellow", edgecolor="darkred"),
)

# Стрелка направления возрастания Z (градиент [8, 10])
gx, gy = 8, 10
norm = np.sqrt(gx**2 + gy**2)
scale = 700
ax_arrow_x, ax_arrow_y = 600, 2800
plt.arrow(
    ax_arrow_x,
    ax_arrow_y,
    gx / norm * scale,
    gy / norm * scale,
    head_width=100,
    head_length=120,
    fc="purple",
    ec="purple",
    linewidth=2,
)
plt.text(
    ax_arrow_x + gx / norm * scale + 60,
    ax_arrow_y + gy / norm * scale + 60,
    "Возрастание Z",
    fontsize=11,
    color="purple",
)

plt.axhline(0, color="black", linewidth=0.8)
plt.axvline(0, color="black", linewidth=0.8)
plt.xlim(0, 4000)
plt.ylim(0, 4000)
plt.xlabel("x₁ (Старый Джек), л/день", fontsize=12)
plt.ylabel("x₂ (Специальное), л/день", fontsize=12)
plt.title(
    "Рисунок 2 – Нахождение оптимального решения\nZ = 8x₁ + 10x₂ → max (прибыль)",
    fontsize=13,
)
plt.legend(fontsize=10)
plt.grid(True, alpha=0.4)
plt.tight_layout()
plt.savefig("graph_optimum.png", dpi=300, bbox_inches="tight")
plt.close()

print(f"Оптимальное решение:")
print(f"  x₁ = {x_opt:.2f} л/день  (Старый Джек)")
print(f"  x₂ = {y_opt:.2f} л/день  (Специальное)")
print(f"  Z_max = {z_opt:.2f} у.е./день")
print("\nСохранено:")
print("  graph_area.png    — пространство допустимых решений")
print("  graph_optimum.png — нахождение оптимального решения")
