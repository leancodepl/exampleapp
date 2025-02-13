import { css, keyframes } from "@pigment-css/react"
import { colors } from "../.."

const pulse = keyframes`
  from {
    background: var(--loading-light);
  }

  to {
    background: var(--loading-dark);
  }
`

export const cssLoading = css`
  --loading-light: ${colors.background.active.tertiary};
  --loading-dark: ${colors.background.active.tertiary_hover};

  position: relative;

  &:before {
    position: absolute;
    top: -1px;
    right: -1px;
    bottom: -1px;
    left: -1px;
    z-index: 2;

    border-radius: inherit;

    animation: ${pulse} 1s linear infinite alternate;

    content: "";
  }
`
