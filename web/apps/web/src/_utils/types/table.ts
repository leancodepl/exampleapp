import { Key } from "react";
import { ColumnType } from "antd/lib/table";
import { Path } from "react-router-dom";

/* eslint-disable @typescript-eslint/no-explicit-any */
type Values<T> = T[keyof T];

export type NoUndefinedField<T> = {
    [P in keyof T]-?: Exclude<T[P], null | undefined>;
};

type StringOrNumber<T> = T & (string | number);

type NestedDataIndices<T, TDepth extends any[]> = Values<{
    [K in keyof T]:
        | [StringOrNumber<K>]
        | (T[K] extends object
              ? TDepth extends [any, ...infer TRest]
                  ? [StringOrNumber<K>, ...NestedDataIndices<NoUndefinedField<T[K]>, TRest>]
                  : never
              : never);
}>;

export type DataIndices<T> = StringOrNumber<keyof T> | NestedDataIndices<NoUndefinedField<T>, [1, 2]>;

export type ResultColumn<TRecord extends { id: Key }> = {
    dataIndex?: DataIndices<TRecord>;
    navigateTo?: (record: TRecord) => string | Partial<Path> | undefined;
} & Omit<ColumnType<TRecord>, "dataIndex">;
