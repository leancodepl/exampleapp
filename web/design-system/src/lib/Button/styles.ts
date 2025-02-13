import { keyframes, styled } from "@pigment-css/react"
import { colors, radii, shadows, spacing } from "../../utils"
import { IconLoading02 } from "../icons"
import { dataBlock, dataDisabled, dataIconOnly, dataSize, dataType } from "./attributes"

export const ButtonRoot = styled("button", {
  name: "Button",
  slot: "root",
})`
  display: inline-flex;
  flex: 0 0 auto;
  gap: ${spacing._2};
  align-items: center;
  height: 2.5rem;
  padding: 0;

  font-weight: 400;
  font-size: 14px;
  line-height: 24px;
  white-space: nowrap;
  text-decoration: none;

  border: none;
  border-radius: ${radii.lg};

  &:not(:disabled):not(${dataDisabled.variant("")}) {
    cursor: pointer;
  }

  &:disabled,
  &${dataDisabled.variant("")} {
    cursor: not-allowed;
  }

  &${dataBlock.variant("")} {
    justify-content: center;
  }

  &${dataSize.variant("large")} {
    height: 3rem;
  }

  &${dataIconOnly.variant("")} {
    justify-content: center;
    width: 2.5rem;

    &${dataSize.variant("large")} {
      width: 3rem;
    }
  }

  &:not(${dataIconOnly.variant("")}):not(${dataType.variant("text")}) {
    padding-inline: ${spacing._6};
  }

  &${dataType.variant("primary")} {
    color: ${colors.foreground.inverse.primary};

    background: ${colors.background.accent.primary};

    &:hover {
      color: ${colors.foreground.inverse.primary_hover};

      background: ${colors.background.accent.primary_hover};
    }

    &:active {
      color: ${colors.foreground.inverse.primary_pressed};

      background: ${colors.background.accent.primary_pressed};
      box-shadow: ${shadows.md};
    }

    &:disabled,
    &${dataDisabled.variant("")} {
      color: ${colors.foreground.disabled.primary};

      background: ${colors.background.disabled.primary};
    }
  }

  &${dataType.variant("secondary")} {
    color: ${colors.foreground.default.primary};

    background: ${colors.background.default.secondary};

    &:hover {
      color: ${colors.foreground.active.primary_hover};

      background: ${colors.background.active.tertiary_hover};
    }

    &:active {
      color: ${colors.foreground.accent.primary_pressed};

      background: ${colors.background.active.tertiary};
      box-shadow: ${shadows.md};
    }

    &:disabled,
    &${dataDisabled.variant("")} {
      color: ${colors.foreground.disabled.primary};

      background: ${colors.background.disabled.tertiary};
    }
  }

  &${dataType.variant("tertiary")} {
    color: ${colors.foreground.active.primary};

    background: transparent;

    &:hover {
      color: ${colors.foreground.active.primary_hover};

      background: ${colors.background.active.tertiary_hover};
    }

    &:active {
      color: ${colors.foreground.active.primary_pressed};

      background: transparent;
      box-shadow: ${shadows.md};
    }

    &:disabled,
    &${dataDisabled.variant("")} {
      color: ${colors.foreground.disabled.primary};
    }
  }

  &${dataType.variant("text")} {
    color: ${colors.foreground.active.primary};

    background: none;

    &:hover {
      color: ${colors.foreground.active.primary_hover};
    }

    &:active {
      color: ${colors.foreground.active.primary_pressed};
    }

    &:disabled,
    &${dataDisabled.variant("")} {
      color: ${colors.foreground.disabled.primary};
    }
  }
`

const rotation = keyframes`
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
`

export const Loader = styled(IconLoading02)<{ shadow?: boolean }>({
  animation: `${rotation} 2s linear infinite`,
  variants: [
    {
      props: { shadow: true },
      style: {
        opacity: 0,
      },
    },
  ],
})
