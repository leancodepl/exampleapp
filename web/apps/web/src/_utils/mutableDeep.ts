export type MutableDeep<T> = T extends string
  ? T
  : T extends ReadonlyArray<infer ArrayType>
  ? Array<MutableDeep<ArrayType>>
  : {
      -readonly [K in keyof T]: MutableDeep<T[K]>;
    };
