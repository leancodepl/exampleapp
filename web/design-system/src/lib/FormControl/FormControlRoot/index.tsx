import { styled } from "@pigment-css/react"
import { spacing } from "../../.."

export const FormControlRoot = styled("div", { name: "FormControl", slot: "root" })`
  display: flex;
  flex-direction: column;
  gap: ${spacing._1};
`
