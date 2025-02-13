import { styled } from "@pigment-css/react"
import * as ToastPrimitive from "@radix-ui/react-toast"
import { breakpoints, spacing } from "../../../utils"

export const SnackbarViewport = styled(ToastPrimitive.Viewport, { name: "Snackbar", slot: "Viewport" })`
  position: fixed;
  right: 0;

  bottom: ${spacing._8};
  left: 0;
  z-index: 2147483647;

  display: flex;
  flex-direction: column;
  align-items: center;
  margin: 0 ${spacing._6};

  list-style: none;
  outline: none;

  pointer-events: none;

  ${breakpoints.up.md} {
    top: ${spacing._6};
    bottom: unset;
  }
`
