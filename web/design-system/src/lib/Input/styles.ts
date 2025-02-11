import { styled } from "@pigment-css/react"
import { colors } from "../.."

export const InputRoot = styled("input", { name: "Input", slot: "root" })`
  flex: 1;
  height: 100%;
  padding: 0;

  color: ${colors.foreground.default.primary};
  font-weight: 400;
  font-size: 14px;
  line-height: 24px;
  letter-spacing: -0.01px;

  background: none;
  border: none;
  outline: none;

  &::placeholder {
    color: ${colors.foreground.default.secondary};
  }
`
