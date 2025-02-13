import { keyframes, styled } from "@pigment-css/react"
import { Slot } from "@radix-ui/react-slot"
import * as ToastPrimitive from "@radix-ui/react-toast"
import { breakpoints, colors, radii, spacing } from "../../../utils"
import { dataType } from "../attributes"

const slideInBottom = keyframes`
  from {
		transform: translateY(calc(100% + 100px));
	}
	to {
		transform: translateY(0);
	}
`

const slideInTop = keyframes`
  from {
		transform: translateY(calc(-100% - 100px));
	}
	to {
		transform: translateY(0);
	}
`

const swipeOut = keyframes`
	from {
		transform: translateX(var(--radix-toast-swipe-end-x));
	}
	to {
		transform: translateX(calc(100% + 100px));
	}
`

export const SnackbarRoot = styled(ToastPrimitive.Root, { name: "Snackbar", slot: "root" })`
  display: flex;
  gap: ${spacing._4};
  align-items: center;
  max-width: 800px;
  padding: ${spacing._4};

  border-radius: ${radii.lg};

  transition: opacity 100ms ease-in;

  pointer-events: auto;

  &${dataType.variant("info")} {
    background: ${colors.background.default.secondary};
  }

  &${dataType.variant("error")} {
    background: ${colors.background.danger.tertiary};
  }

  &${dataType.variant("success")} {
    background: ${colors.background.success.tertiary};
  }

  &[data-state="open"] {
    animation: ${slideInBottom} 150ms cubic-bezier(0.16, 1, 0.3, 1);

    ${breakpoints.up.md} {
      animation: ${slideInTop} 150ms cubic-bezier(0.16, 1, 0.3, 1);
    }
  }

  &[data-state="closed"] {
    opacity: 0;
  }

  &[data-swipe="move"] {
    transform: translateX(var(--radix-toast-swipe-move-x));
  }

  &[data-swipe="end"] {
    animation: ${swipeOut} 100ms ease-out;
  }
`

export const SnackbarIcon = styled(Slot, { name: "Snackbar", slot: "icon" })`
  ${dataType.variant("error")} & {
    color: ${colors.foreground.danger.primary};
  }
`
