import { ComponentType, Fragment, ReactNode, useCallback, useState } from "react"
import { Breakpoint } from "@pigment-css/react"
import { ColumnDef, flexRender, getCoreRowModel, RowData, TableMeta, useReactTable } from "@tanstack/react-table"
import classNames from "classnames"
import { cssLoading } from "../../utils/styles/loading"
import * as Dialog from "../Dialog"
import { Flex } from "../Flex"
import { IconChevronRight } from "../icons"
import { cssMedia } from "../Media"
import { Text } from "../Text"
import { EmptyStateCell, TableCell, TableHeaderCell, TableRoot, TableRow, TableTriggerButton } from "./styles"

type RowKeyFunc<T> = { rowKey: (item: T) => string }
type RowKeyProps<T> = T extends { id: string } ? Partial<RowKeyFunc<T>> : RowKeyFunc<T>
export type TableModalProps<T> = { item: T; close: () => void }
export type LinkProps<T> = { item: T; children?: ReactNode; className?: string }

type TableProps<T> = {
  "data-testid"?: string
  meta?: TableMeta<T>
  data?: T[]
  columns: ColumnDef<T>[]
  modal?: ComponentType<TableModalProps<T>>
  modalTo?: Breakpoint
  link?: ComponentType<LinkProps<T>>
  rowClassName?: (item: T) => string
  isLoading?: boolean
  emptyState?: ReactNode
} & RowKeyProps<T>

const emptyArray: any[] = []

export function Table<T>({
  meta,
  data = emptyArray,
  rowKey = item => item.id,
  columns,
  modal: Modal,
  modalTo,
  link: LinkComponent,
  rowClassName,
  isLoading,
  emptyState,
  "data-testid": dataTestId,
}: TableProps<T>) {
  const table = useReactTable({
    data,
    columns,
    meta,
    defaultColumn: {
      enableSorting: false,
    },
    getCoreRowModel: getCoreRowModel(),
    getRowId: rowKey,
  })

  const rows = table.getRowModel().rows

  return (
    <TableRoot className={classNames(isLoading && cssLoading)} data-testid={dataTestId}>
      <thead>
        <TableRow>
          {table.getHeaderGroups().map(headerGroup => (
            <Fragment key={headerGroup.id}>
              {headerGroup.headers.map(header => (
                <TableHeaderCell
                  key={header.id}
                  as="th"
                  className={cssMedia({ from: header.column.columnDef.meta?.from })}
                  style={{ width: header.column.columnDef.meta?.width }}>
                  {header.isPlaceholder ? null : (
                    <Flex align="center" gap="small">
                      <Text caption>{flexRender(header.column.columnDef.header, header.getContext())} </Text>
                    </Flex>
                  )}
                </TableHeaderCell>
              ))}
            </Fragment>
          ))}
          {Modal && <TableHeaderCell as="th" className={cssMedia({ to: modalTo })} style={{ width: 72 }} />}
          {LinkComponent && <TableHeaderCell as="th" style={{ width: 72 }} />}
        </TableRow>
      </thead>

      <tbody style={{ height: isLoading ? 300 : undefined }}>
        {rows.map(row => (
          <TableRow key={row.id} className={rowClassName?.(row.original)}>
            {row.getVisibleCells().map(cell => (
              <TableCell key={cell.id} className={cssMedia({ from: cell.column.columnDef.meta?.from })}>
                <Text body color="default.primary">
                  {flexRender(cell.column.columnDef.cell, cell.getContext())}
                </Text>
              </TableCell>
            ))}

            {Modal && (
              <TableCell className={cssMedia({ to: modalTo })}>
                <TableDialog item={row.original} modal={Modal} />
              </TableCell>
            )}
            {LinkComponent && (
              <TableCell>
                <TableTriggerButton asChild type="text">
                  <LinkComponent item={row.original}>
                    <IconChevronRight />
                  </LinkComponent>
                </TableTriggerButton>
              </TableCell>
            )}
          </TableRow>
        ))}
        {emptyState && data.length === 0 && (
          <TableRow>
            <EmptyStateCell>
              <Text body color="default.secondary">
                {emptyState}
              </Text>
            </EmptyStateCell>
            <TableCell />
          </TableRow>
        )}
      </tbody>
    </TableRoot>
  )
}

export * from "./mkCols"

declare module "@tanstack/table-core" {
  // eslint-disable-next-line @typescript-eslint/no-unused-vars, unused-imports/no-unused-vars
  interface ColumnMeta<TData extends RowData, TValue> {
    cellClassName?: string
    from?: Breakpoint
    width?: number | string
  }
}

type TableDialogProps<T> = {
  item: T
  modal: ComponentType<TableModalProps<T>>
}

function TableDialog<T>({ modal: Modal, item }: TableDialogProps<T>) {
  const [open, setOpen] = useState(false)

  const close = useCallback(() => setOpen(false), [])

  return (
    <Dialog.Root open={open} onOpenChange={setOpen}>
      <Dialog.Trigger asChild>
        <TableTriggerButton leading={<IconChevronRight />} type="text" />
      </Dialog.Trigger>

      <Modal close={close} item={item} />
    </Dialog.Root>
  )
}
