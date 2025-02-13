import { styled } from "@pigment-css/react"
import * as AccordionPrimitive from "@radix-ui/react-accordion"
import { colors, spacing } from "../../../utils"

export const AccordionPrimitiveHeader = styled(AccordionPrimitive.Header)`
  margin: 0;
`

export const AccordionPrimitiveTrigger = styled(AccordionPrimitive.Trigger)`
  display: flex;
  gap: ${spacing._4};
  align-items: center;
  justify-content: space-between;
  width: 100%;
  padding: ${spacing._4};

  text-align: left;

  background: none;
  border: none;
  cursor: pointer;

  &:hover {
    background: ${colors.background.default.primary_hover};
  }
`

export const AccordionPrimitiveIndicator = styled.div`
  svg {
    color: ${colors.background.accent.primary};

    transition: transform ease-out 0.2s;

    [data-state="open"] & {
      transform: rotate(-180deg);
    }
  }
`
