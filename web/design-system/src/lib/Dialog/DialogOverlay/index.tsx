import { keyframes, styled } from "@pigment-css/react"
import * as DialogPrimitive from "@radix-ui/react-dialog"
import { breakpoints } from "../../.."

const overlayShow = keyframes`
  from {
		opacity: 0;
	}
	to {
		opacity: 1;
	}
`

export const DialogOverlay = styled(DialogPrimitive.Overlay, { name: "Dialog", slot: "overlay" })`
  position: fixed;
  z-index: 20;

  background: rgba(0, 0, 0, 0.3);

  animation: ${overlayShow} 150ms cubic-bezier(0.16, 1, 0.3, 1);
  inset: 0;

  ${breakpoints.up.xs} {
    backdrop-filter: blur(28px);
  }
`
