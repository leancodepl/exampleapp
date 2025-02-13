import { styled } from "@pigment-css/react"
import * as SheetPrimitive from "@radix-ui/react-dialog"

export const SheetTitle = styled(SheetPrimitive.Title, { name: "Sheet", slot: "title" })(() => ({}))

export const SheetDescription = styled(SheetPrimitive.Description, { name: "Sheet", slot: "description" })(() => ({}))

export const SheetFooter = styled("div", { name: "Sheet", slot: "footer" })(() => ({
  display: "flex",
  flexDirection: "column-reverse",
}))

export const SheetHeader = styled("div", { name: "Sheet", slot: "header" })(() => ({
  display: "flex",
  flexDirection: "column",
  textAlign: "center",
}))
