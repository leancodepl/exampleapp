import { keyframes, styled } from "@pigment-css/react"
import * as DialogPrimitive from "@radix-ui/react-dialog"
import { breakpoints, colors, radii, spacing } from "../../../utils"

const contentShowDesktop = keyframes`
  from {
    opacity: 0;
    transform: translate(-50%, -48%) scale(0.9);
  }
  to {
    opacity: 1;
    transform: translate(-50%, -50%) scale(1);
  }
`

const contentShowMobile = keyframes`
  from {
    transform: translate(0, 0%);
  }
  to {
    opacity: 1;
    transform: translateY(calc(-100% + (100lvh - 100svh)));
  }
`

export const StyledDialogContent = styled(DialogPrimitive.DialogContent, { name: "Dialog", slot: "content" })`
  position: fixed;
  z-index: 30;

  display: flex;
  flex-direction: column;
  width: 100%;
  max-height: 85vh;

  background-color: ${colors.background.default.primary};
  border-top-left-radius: ${radii.xl};
  border-top-right-radius: ${radii.xl};

  ${breakpoints.up.xs} {
    top: 50%;
    left: 50%;

    max-width: 84vw;

    border-bottom-right-radius: ${radii.xl};
    border-bottom-left-radius: ${radii.xl};
    transform: translate(-50%, -50%);

    animation: ${contentShowDesktop} 150ms cubic-bezier(0.16, 1, 0.3, 1);
  }

  ${breakpoints.down.xs} {
    top: 100dvh;

    padding-top: ${spacing._4};
    padding-bottom: calc(100lvh - 100svh);

    transform: translateY(calc(-100% + (100lvh - 100svh)));

    animation: ${contentShowMobile} 200ms cubic-bezier(0.16, 1, 0.3, 1);

    &:before {
      position: absolute;
      top: 6px;
      left: 50%;

      width: 64px;
      height: 4px;

      background: ${colors.foreground.default.tertiary};
      border-radius: 2px;
      transform: translateX(-50%);

      content: "";
    }

    /* this is dirty safari only hack */
    @media not all and (min-resolution: 0.001dpcm) {
      @supports (-webkit-appearance: none) {
        top: 100svh;
      }
    }
  }

  ${breakpoints.up.lg} {
    max-width: 60vw;
  }

  ${breakpoints.up.xl} {
    max-width: 48vw;
  }
`
