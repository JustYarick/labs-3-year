import math


def ipart(x):
    return int(math.floor(x))


def roundn(x):
    return ipart(x + 0.5)


def fpart(x):
    return x - math.floor(x)


def rfpart(x):
    return 1 - fpart(x)


def draw_line_wu(x1, y1, x2, y2):
    pixels = []

    steep = abs(y2 - y1) > abs(x2 - x1)

    if steep:
        x1, y1 = y1, x1
        x2, y2 = y2, x2

    if x1 > x2:
        x1, x2 = x2, x1
        y1, y2 = y2, y1

    dx = x2 - x1
    dy = y2 - y1
    gradient = dy / dx if dx != 0 else 0

    # ---- Первая точка ----
    xend = roundn(x1)
    yend = y1 + gradient * (xend - x1)
    xgap = rfpart(x1 + 0.5)
    xpxl1 = xend
    ypxl1 = ipart(yend)

    if steep:
        pixels.append((ypxl1, xpxl1, rfpart(yend) * xgap))
        pixels.append((ypxl1 + 1, xpxl1, fpart(yend) * xgap))
    else:
        pixels.append((xpxl1, ypxl1, rfpart(yend) * xgap))
        pixels.append((xpxl1, ypxl1 + 1, fpart(yend) * xgap))

    intery = yend + gradient

    # ---- Вторая точка ----
    xend = roundn(x2)
    yend = y2 + gradient * (xend - x2)
    xgap = fpart(x2 + 0.5)
    xpxl2 = xend
    ypxl2 = ipart(yend)

    if steep:
        pixels.append((ypxl2, xpxl2, rfpart(yend) * xgap))
        pixels.append((ypxl2 + 1, xpxl2, fpart(yend) * xgap))
    else:
        pixels.append((xpxl2, ypxl2, rfpart(yend) * xgap))
        pixels.append((xpxl2, ypxl2 + 1, fpart(yend) * xgap))

    # ---- Основной цикл ----
    for x in range(xpxl1 + 1, xpxl2):
        if steep:
            pixels.append((ipart(intery), x, rfpart(intery)))
            pixels.append((ipart(intery) + 1, x, fpart(intery)))
        else:
            pixels.append((x, ipart(intery), rfpart(intery)))
            pixels.append((x, ipart(intery) + 1, fpart(intery)))

        intery += gradient

    return pixels


def main():
    points = draw_line_wu(10, 20, 200, 150)

    for point in points:
        print(f"Pixel: ({point[0]}, {point[1]}), Intensity: {point[2]:.2f}")


if __name__ == "__main__":
    main()
