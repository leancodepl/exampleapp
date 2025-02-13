import { styled } from "@pigment-css/react"
import { colors, radii, spacing } from "../../utils"
import { dataError } from "../FormControl/attributes"

export const CheckboxContainer = styled.label`
  display: flex;
  flex: none;
  gap: 0 ${spacing._2};
  align-items: flex-start;
`

export const CheckInput = styled.input`
  flex: none;
  width: 24px;
  height: 18px;
  margin: 0;

  appearance: none;

  &:focus-visible {
    outline: none;
  }
`

export const CheckIndicator = styled.svg`
  --color: ${colors.foreground.default.secondary};

  position: absolute;

  flex: none;
  margin: 3px;

  color: var(--color);

  border: 2px solid var(--color);
  border-radius: ${radii.sm};
  cursor: pointer;

  transition: all 100ms ease-in-out;

  &:hover {
    --color: ${colors.foreground.accent.primary_hover};
  }

  > path {
    transform: scale(0);
    transform-origin: 50% 50%;

    transition: inherit;

    stroke: var(--color);
  }

  input:checked + & {
    --color: ${colors.foreground.accent.primary};
    background: ${colors.background.accent.tertiary};

    > path {
      transform: scale(1);
    }

    &:hover {
      background: ${colors.background.accent.tertiary_hover};
    }
  }

  input:active + & {
    --color: ${colors.foreground.accent.primary_pressed};
  }

  input:active:checked + & {
    background: ${colors.background.accent.tertiary_pressed};
  }

  ${dataError.variant("")} input + & {
    --color: ${colors.foreground.danger.primary};

    background: ${colors.background.danger.tertiary};

    &:hover {
      background: ${colors.background.danger.tertiary};
    }
  }
`
