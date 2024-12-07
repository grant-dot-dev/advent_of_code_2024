def equals_target(target, numbers):
    def backtrack(index, current_value):
        if index == len(numbers):
            return current_value == target

        # Try addition
        if backtrack(index + 1, current_value + numbers[index]):
            return True

        # Try multiplication
        if backtrack(index + 1, current_value * numbers[index]):
            return True

        return False

    return backtrack(1, numbers[0])


total = 0
with open('../input1.txt', 'r') as file:
    lines = file.readlines()


for line in lines:
    test_value, number_list = line.strip().split(':')
    target = int(test_value)
    numbers = list(map(int, number_list.split()))
    total += target if equals_target(target, numbers) else 0

print({total})
