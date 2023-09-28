import {
  ApiDateOnly,
  ApiDateTimeOffset,
  ApiTimeOnly,
  ApiTimeSpan,
} from '@leancodepl/api-date-datefns';

export type UncapitalizeDeep<T> = T extends Array<infer TArrayElement>
  ? Array<UncapitalizeDeep<TArrayElement>>
  : T extends ApiDateTimeOffset | ApiDateOnly | ApiTimeOnly | ApiTimeSpan
  ? T
  : T extends object
  ? {
      [TKey in keyof T as TKey extends string
        ? Uncapitalize<TKey>
        : TKey]: UncapitalizeDeep<T[TKey]>;
    }
  : T extends null
  ? undefined
  : T;
