def cycle(secret_number):
    secret = int(secret_number)
    secret = evolve_secret_number(secret)
    return secret


def evolve_secret_number(secret):
    r1 = secret * 64
    secret ^= r1
    secret %= 16777216

    r2 = secret // 32
    secret ^= r2
    secret %= 16777216

    r3 = secret * 2048
    secret ^= r3
    secret %= 16777216

    return secret


def part1():
    secrets = []
    with open("input.txt", "r") as file:
        input = [line for line in file.read().splitlines() if line.strip()]

    for line in input:
        secret = int(line)
        for i in range(2000):
            secret = cycle(secret)

        secrets.append(secret)

    print(f"Part 1: {sum(secrets)}")


if __name__ == "__main__":
    part1()
