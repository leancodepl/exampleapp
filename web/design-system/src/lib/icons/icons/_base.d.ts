declare module "*.svg?icon" {
  import * as React from "react"

  type IconProps = {
    title?: string
    size?: "illustration" | "small" | "standard"
  } & React.ComponentProps<"svg">

  const ReactComponent: React.FunctionComponent<IconProps>

  export default ReactComponent
}
