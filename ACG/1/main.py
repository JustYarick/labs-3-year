def bresenham_line(x0, y0, x1, y1):
    points = []

    dx = abs(x1 - x0)
    dy = abs(y1 - y0)

    sx = 1 if x0 < x1 else -1
    sy = 1 if y0 < y1 else -1

    err = dx - dy

    while True:
        points.append((x0, y0))

        if x0 == x1 and y0 == y1:
            break

        e2 = 2 * err

        if e2 > -dy:
            err -= dy
            x0 += sx

        if e2 < dx:
            err += dx
            y0 += sy

    return points


def main():
    x0 = 0
    y0 = 0

    x1 = 5
    y1 = 5

    result = bresenham_line(x0, y0, x1, y1)

    for point in result:
        print(point)


if __name__ == "__main__":
    main()
