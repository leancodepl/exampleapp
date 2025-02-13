export type MutuallyExclusiveUnion<T extends number | string | symbol> = Values<{
  [K in T]: Partial<Record<Exclude<T, K>, false>> & Record<K, true>
}>

type Values<T> = T[keyof T]
