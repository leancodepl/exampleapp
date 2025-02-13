import { styled } from "@pigment-css/react"
import * as SheetPrimitive from "@radix-ui/react-dialog"
import { colors } from "../../.."

export const SheetOverlay = styled(SheetPrimitive.Overlay, { name: "Sheet", slot: "overlay" })(() => ({
  position: "fixed",
  inset: 0,
  zIndex: 50,
}))

export const StyledSheetContent = styled(SheetPrimitive.Content, { name: "Sheet", slot: "content" })`
  position: fixed;
  top: 0;
  left: 0;
  z-index: 50;

  width: 100vw;
  height: 100dvh;

  background-color: ${colors.background.default.primary};
`
