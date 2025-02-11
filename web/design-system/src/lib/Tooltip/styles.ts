import { styled } from "@pigment-css/react"
import * as TooltipPrimitive from "@radix-ui/react-tooltip"
import { colors, radii, spacing } from "../.."

export const TooltipArrow = styled(TooltipPrimitive.Arrow, { name: "Tooltip", slot: "arrow" })`
  position: relative;

  border-top: 8px solid var(--tooltip-border);
  border-right: 8px solid transparent;
  border-left: 8px solid transparent;

  &::after {
    position: absolute;

    border: inherit;
    border-top-color: var(--tooltip-background);
    transform: translate(-50%, -100%) translate(0, -1.5px);

    content: "";
  }
`

export const TooltipContent = styled(TooltipPrimitive.Content, { name: "Tooltip", slot: "content" })`
  z-index: 40;

  display: flex;
  align-items: center;
  max-width: 500;
  min-height: 2rem;
  padding: ${spacing._1} ${spacing._2};

  color: var(--tooltip-color);
  white-space: normal;

  background: var(--tooltip-background);
  border: 1px solid var(--tooltip-border);
  border-radius: ${radii.lg};

  [data-type="warning"]& {
    --tooltip-background: ${colors.background.warning.tertiary};
    --tooltip-border: ${colors.foreground.warning.secondary};
    --tooltip-color: ${colors.foreground.warning.primary};
  }

  [data-type="info"]& {
    --tooltip-background: ${colors.background.info.tertiary};
    --tooltip-border: ${colors.foreground.info.secondary};
    --tooltip-color: ${colors.foreground.info.primary};
  }

  [data-type="success"]& {
    --tooltip-background: ${colors.background.success.tertiary};
    --tooltip-border: ${colors.foreground.success.secondary};
    --tooltip-color: ${colors.foreground.success.primary};
  }

  [data-type="danger"]& {
    --tooltip-background: ${colors.background.danger.tertiary};
    --tooltip-border: ${colors.foreground.danger.secondary};
    --tooltip-color: ${colors.foreground.danger.primary};
  }

  [data-type="inverse"]& {
    --tooltip-background: ${colors.background.inverse.primary};
    --tooltip-border: ${colors.foreground.default.secondary};
    --tooltip-color: ${colors.foreground.inverse.primary};
  }
`
