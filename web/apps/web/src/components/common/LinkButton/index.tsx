import { To, useHref, useLinkClickHandler } from "react-router-dom"
import { Button, ButtonProps } from "antd"

type LinkButtonProps = (
    | {
          replace?: undefined
          state?: undefined
          external: true
          href: string
      }
    | { replace?: boolean; state?: unknown; href: To; external?: false }
) &
    Omit<ButtonProps, "href">

export function LinkButton({ href, replace, state, target, onClick, external, ...props }: LinkButtonProps) {
    const linkHref = useHref(href)
    const internalOnClick = useLinkClickHandler(href, { replace, state, target })

    const handleClick = (event: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => {
        onClick?.(event)

        if (!event.defaultPrevented) {
            internalOnClick(event)
        }
    }

    return (
        <Button
            {...props}
            href={external ? href : linkHref}
            target={target}
            onClick={(external ? onClick : handleClick) as ButtonProps["onClick"]}
        />
    )
}
