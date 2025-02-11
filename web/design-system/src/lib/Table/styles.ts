import { styled } from "@pigment-css/react"
import { colors, radii, spacing } from "../../utils"
import { Button } from "../Button"

export const TableRoot = styled("table", { name: "Table", slot: "root" })`
  position: relative;

  width: 100%;

  table-layout: fixed;
  border: 1px solid ${colors.foreground.default.tertiary};
  border-radius: ${radii.md};
  border-collapse: separate;
  border-spacing: 0;
`

export const TableRow = styled("tr", { name: "Table", slot: "row" })``

export const TableCellBase = styled.td`
  height: 56px;
  padding: ${spacing._2} ${spacing._4};
  overflow: hidden;

  white-space: nowrap;
  text-overflow: ellipsis;

  tbody > tr:not(:last-child) > &,
  thead > tr > & {
    border-bottom: 1px solid ${colors.foreground.default.tertiary};
  }
`

export const TableHeaderCell = styled(TableCellBase, { name: "Table", slot: "headerCell" })`
  height: 48px;

  color: ${colors.foreground.default.secondary};
`

export const TableCell = styled(TableCellBase, { name: "Table", slot: "cell" })`
  color: ${colors.foreground.default.primary};
`

export const TableTriggerButton = styled(Button, { name: "Table", slot: "modalTriggerButton" })`
  &&& {
    padding-inline: 0;

    color: ${colors.background.accent.primary};
  }
`

export const EmptyStateCell = styled(TableCell)`
  position: absolute;

  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
`
