import { LinkComponentProps } from "@leancodepl/kratos"

export function AuthLink({ node: _node, icon, children, ...props }: LinkComponentProps) {
  return (
    <a {...props}>
      {icon} {children}
    </a>
  )
}
