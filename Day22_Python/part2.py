from collections import defaultdict


def cycle(secret):
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


def process_input(filename):
    with open(filename, "r") as file:
        input_lines = [line.strip() for line in file if line.strip()]

    return list(map(int, input_lines))


def secret_sequences(secret_numbers, generations):
    sequence_sum = defaultdict(int)

    for secret in secret_numbers:
        generated_secrets = calc_secrets(secret, generations)
        calc_price_changes(generated_secrets, sequence_sum)

    return sequence_sum


def calc_price_changes(generated_secrets, sequence_sum):
    sequences = set()
    price_change_sequence = []
    prev_price = generated_secrets[0] % 10

    # Calculate the price changes
    for secret in generated_secrets[1:]:
        price = secret % 10
        change = price - prev_price
        price_change_sequence.append((change, price))
        prev_price = price

    # Save price for each 4 price changes
    for p in range(len(price_change_sequence) - 4 + 1):
        changes = price_change_sequence[p: p + 4]
        key = tuple((changes[0][0], changes[1][0],
                    changes[2][0], changes[3][0]))
        if key not in sequences:
            sequence_sum[key] += changes[3][1]
            sequences.add(key)


def calc_secrets(secret, generations):
    """Generate a sequence of secrets by iterating 'generations' times"""
    generated_secrets = [secret]
    for _ in range(generations):
        secret = cycle(secret)
        generated_secrets.append(secret)

    return generated_secrets


def part2():
    filename = 'input.txt'
    generations = 2000

    secret_numbers = process_input(filename)
    sequence_sum = secret_sequences(secret_numbers, generations)
    best_sequence = max(sequence_sum.values())

    print()
    print('Most bananas =', best_sequence)


if __name__ == "__main__":
    part2()
