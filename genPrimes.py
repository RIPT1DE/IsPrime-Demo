PRIMES_RANGE = [0, 1000]

primes = []


def is_prime(n):
    if n == 0:
      return False
  
    if n == 1:
        return False

    if n == 2:
        return True

    return all(n % i != 0 for i in primes)


def gen_primes():
    for num in range(PRIMES_RANGE[0], PRIMES_RANGE[1]):
        if is_prime(num):
            primes.append(num)


if __name__ == "__main__":
    gen_primes()

    with open("primes.txt", "w", encoding="utf-8") as f:
        f.writelines(str(prime) + "\n" for prime in primes)
