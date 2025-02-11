import { styled } from "@pigment-css/react"
import * as RadioGroupPrimitive from "@radix-ui/react-radio-group"
import { colors, spacing } from "../../../utils"

export const RadioItemWrapper = styled(RadioGroupPrimitive.Item, { name: "Radio", slot: "item" })`
  display: flex;
  gap: ${spacing._2};

  background: none;
  cursor: pointer;
`

export const RadioIndicator = styled(RadioGroupPrimitive.Indicator, { name: "Radio", slot: "indicator" })`
  --color: ${colors.foreground.accent.primary};
  position: relative;

  display: block;
  width: 20px;
  height: 20px;
  margin: 2px;

  border: 2px solid var(--color);
  border-radius: 100%;

  &[data-state="checked"]:after {
    position: absolute;
    top: 50%;
    left: 50%;

    width: 10px;
    height: 10px;

    background: var(--color);
    border-radius: 100%;
    transform: translate(-50%, -50%);

    content: "";
  }

  [role="radio"]:hover & {
    --color: ${colors.foreground.accent.primary_hover};
  }

  [role="radio"]:active & {
    --color: ${colors.foreground.accent.primary_pressed};
  }
`
