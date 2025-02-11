import { useEffect } from "react"
import { ReactNode } from "@tanstack/react-router"
import { Text } from "../../Text"
import { useFormControl } from "../FormControlProvider"
import { FormControlLabelWrapper } from "./styles"

type FormControlLabelProps = {
  children?: ReactNode
}

export function FormControlLabel({ children }: FormControlLabelProps) {
  const { setLabel } = useFormControl()

  useEffect(() => setLabel(children), [children, setLabel])

  return (
    <FormControlLabelWrapper>
      <Text caption>{children}</Text>
    </FormControlLabelWrapper>
  )
}
