from collections import Counter


def part_1(list1, list2):
    totals = [abs(value - partner_item)
              for value, partner_item in zip(list1, list2)]
    print("Sum Total:", sum(totals))


def part_2(list1, list2):
    counts = Counter(list2)
    total_sum = sum(number * counts[number] for number in list1)
    print(f"Sum of counts: {total_sum}")


def read_input(file_name):
    list1, list2 = zip(*((int(num1), int(num2))
                       for num1, num2 in (line.split() for line in open(file_name))))
    return sorted(list1), sorted(list2)


list1, list2 = read_input("./input.txt")

part_1(list1, list2)
part_2(list1, list2)
