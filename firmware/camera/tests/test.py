import re
import sys
import serial
import pygame

SERIAL_PORT = "/dev/cu.usbserial-A5069RR4"
BAUD_RATE = 115200
WINDOW_WIDTH = 1920
WINDOW_HEIGHT = 1080
DOT_RADIUS = 12

LINE_RE = re.compile(rb"POS,(-?[\d.]+),(-?[\d.]+),([01])")


def clamp(value, lo=0.0, hi=1.0):
    return max(lo, min(hi, value))


def main():
    try:
        ser = serial.Serial(SERIAL_PORT, BAUD_RATE, timeout=0.1)
        ser.dtr = False
        ser.rts = False
    except serial.SerialException as e:
        print(f"Could not open serial port {SERIAL_PORT}: {e}")
        print("Check the port name/permissions and try again.")
        sys.exit(1)

    pygame.init()
    screen = pygame.display.set_mode((WINDOW_WIDTH, WINDOW_HEIGHT))
    pygame.display.set_caption("Light Gun Position")
    clock = pygame.time.Clock()
    font = pygame.font.SysFont(None, 24)

    col, row, found = 0.5, 0.5, False
    last_update_ms = pygame.time.get_ticks()

    running = True
    while running:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False
            elif event.type == pygame.KEYDOWN and event.key == pygame.K_ESCAPE:
                running = False

        try:
            while ser.in_waiting:
                line = ser.readline()
                match = LINE_RE.search(line)
                if match:
                    col = clamp(float(match.group(1)))
                    row = clamp(float(match.group(2)))
                    found = match.group(3) == b"1"
                    last_update_ms = pygame.time.get_ticks()
        except serial.SerialException as e:
            print(f"Serial read error: {e}")
            running = False

        x = int((1 - col) * WINDOW_WIDTH)
        y = int((1 - row) * WINDOW_HEIGHT)

        stale_ms = pygame.time.get_ticks() - last_update_ms
        color = (0, 220, 0) if found else (220, 60, 60)
        if stale_ms > 500:
            color = (120, 120, 120)

        screen.fill((20, 20, 20))

        pygame.draw.line(screen, (60, 60, 60), (0, y), (WINDOW_WIDTH, y), 1)
        pygame.draw.line(screen, (60, 60, 60), (x, 0), (x, WINDOW_HEIGHT), 1)

        pygame.draw.circle(screen, color, (x, y), DOT_RADIUS)

        info = font.render(
            f"col={col:.3f} row={row:.3f} found={found} stale={stale_ms}ms",
            True,
            (230, 230, 230),
        )
        screen.blit(info, (10, 10))

        pygame.display.flip()
        clock.tick(120)

    ser.close()
    pygame.quit()


if __name__ == "__main__":
    main()
