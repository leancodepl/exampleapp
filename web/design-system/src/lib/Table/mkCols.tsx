/* eslint-disable no-redeclare */
import { ColumnDef } from "@tanstack/react-table"

export function mkCols<TData>() {
  function cols<TValue1>(column1: ColumnDef<TData, TValue1>): ColumnDef<TData>[]
  function cols<TValue1, TValue2>(
    column1: ColumnDef<TData, TValue1>,
    column2: ColumnDef<TData, TValue2>,
  ): ColumnDef<TData>[]
  function cols<TValue1, TValue2, TValue3>(
    column1: ColumnDef<TData, TValue1>,
    column2: ColumnDef<TData, TValue2>,
    column3: ColumnDef<TData, TValue3>,
  ): ColumnDef<TData>[]
  function cols<TValue1, TValue2, TValue3, TValue4>(
    column1: ColumnDef<TData, TValue1>,
    column2: ColumnDef<TData, TValue2>,
    column3: ColumnDef<TData, TValue3>,
    column4: ColumnDef<TData, TValue4>,
  ): ColumnDef<TData>[]
  function cols<TValue1, TValue2, TValue3, TValue4, TValue5>(
    column1: ColumnDef<TData, TValue1>,
    column2: ColumnDef<TData, TValue2>,
    column3: ColumnDef<TData, TValue3>,
    column4: ColumnDef<TData, TValue4>,
    column5: ColumnDef<TData, TValue5>,
  ): ColumnDef<TData>[]
  function cols<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
    column1: ColumnDef<TData, TValue1>,
    column2: ColumnDef<TData, TValue2>,
    column3: ColumnDef<TData, TValue3>,
    column4: ColumnDef<TData, TValue4>,
    column5: ColumnDef<TData, TValue5>,
    column6: ColumnDef<TData, TValue6>,
  ): ColumnDef<TData>[]
  function cols<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
    column1: ColumnDef<TData, TValue1>,
    column2: ColumnDef<TData, TValue2>,
    column3: ColumnDef<TData, TValue3>,
    column4: ColumnDef<TData, TValue4>,
    column5: ColumnDef<TData, TValue5>,
    column6: ColumnDef<TData, TValue6>,
    column7: ColumnDef<TData, TValue7>,
  ): ColumnDef<TData>[]

  function cols(...columns: ColumnDef<TData, any>[]): ColumnDef<TData>[]
  function cols(...columns: ColumnDef<TData>[]): ColumnDef<TData>[] {
    return columns
  }

  return cols
}
