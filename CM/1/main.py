from itertools import combinations

fats = [330, 240, 300]
proteins = [170, 120, 110]
carbs = [380, 440, 490]
price = [31, 23, 20]

req_fats = 800
req_prot = 700
req_carbs = 900


def solve3x3(A, b):
    """Решение системы 3x3 методом Гаусса"""
    A = [row[:] for row in A]
    b = b[:]

    n = 3
    for i in range(n):
        piv = max(range(i, n), key=lambda r: abs(A[r][i]))
        if abs(A[piv][i]) < 1e-12:
            return None

        # перестановка строк
        A[i], A[piv] = A[piv], A[i]
        b[i], b[piv] = b[piv], b[i]

        # нормализация строки
        div = A[i][i]
        for j in range(i, n):
            A[i][j] /= div
        b[i] /= div

        # зануление остальных
        for r in range(n):
            if r != i:
                factor = A[r][i]
                for j in range(i, n):
                    A[r][j] -= factor * A[i][j]
                b[r] -= factor * b[i]

    return b


# ограничения
constraints = [
    ([330, 240, 300], 800),  # жиры
    ([170, 120, 110], 700),  # белки
    ([380, 440, 490], 900),  # углеводы
    ([1, 0, 0], 0),  # xA = 0
    ([0, 1, 0], 0),  # xB = 0
    ([0, 0, 1], 0),  # xC = 0
]

best_cost = float("inf")
best = None

for combo in combinations(constraints, 3):
    A = [combo[0][0], combo[1][0], combo[2][0]]
    b = [combo[0][1], combo[1][1], combo[2][1]]

    sol = solve3x3(A, b)
    if sol is None:
        continue

    xA, xB, xC = sol

    # проверка неотрицательности
    if xA < -1e-6 or xB < -1e-6 or xC < -1e-6:
        continue

    # проверка ограничений
    fats_val = fats[0] * xA + fats[1] * xB + fats[2] * xC
    prot_val = proteins[0] * xA + proteins[1] * xB + proteins[2] * xC
    carb_val = carbs[0] * xA + carbs[1] * xB + carbs[2] * xC

    if fats_val + 1e-6 < req_fats:
        continue
    if prot_val + 1e-6 < req_prot:
        continue
    if carb_val + 1e-6 < req_carbs:
        continue

    cost = price[0] * xA + price[1] * xB + price[2] * xC

    if cost < best_cost:
        best_cost = cost
        best = (xA, xB, xC)


if best is None:
    print("Решение не найдено")
else:
    print("Корм A =", round(best[0], 4))
    print("Корм B =", round(best[1], 4))
    print("Корм C =", round(best[2], 4))
    print("Минимальная стоимость =", round(best_cost, 4))
