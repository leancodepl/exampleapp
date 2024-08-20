import { Key } from "react"
import { Path } from "react-router-dom"
import { ColumnType } from "antd/lib/table"

/* eslint-disable @typescript-eslint/no-explicit-any */
type Values<T> = T[keyof T]

export type NoUndefinedField<T> = {
    [P in keyof T]-?: Exclude<T[P], null | undefined>
}

type StringOrNumber<T> = T & (number | string)

type NestedDataIndices<T, TDepth extends any[]> = Values<{
    [K in keyof T]:
        | [StringOrNumber<K>]
        | (T[K] extends object
              ? TDepth extends [any, ...infer TRest]
                  ? [StringOrNumber<K>, ...NestedDataIndices<NoUndefinedField<T[K]>, TRest>]
                  : never
              : never)
}>

export type DataIndices<T> = NestedDataIndices<NoUndefinedField<T>, [1, 2]> | StringOrNumber<keyof T>

export type ResultColumn<TRecord extends { id: Key }> = {
    dataIndex?: DataIndices<TRecord>
    navigateTo?: (record: TRecord) => Partial<Path> | string | undefined
} & Omit<ColumnType<TRecord>, "dataIndex">
