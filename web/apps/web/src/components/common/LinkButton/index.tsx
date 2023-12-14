import { Button, ButtonProps } from "antd";
import { To, useHref, useLinkClickHandler } from "react-router-dom";

type LinkButtonProps = Omit<ButtonProps, "href"> &
    (
        | { replace?: boolean; state?: unknown; href: To; external?: false }
        | {
              replace?: undefined;
              state?: undefined;
              external: true;
              href: string;
          }
    );

export function LinkButton({ href, replace, state, target, onClick, external, ...props }: LinkButtonProps) {
    const linkHref = useHref(href);
    const internalOnClick = useLinkClickHandler(href, { replace, state, target });

    const handleClick = (event: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => {
        onClick?.(event);

        if (!event.defaultPrevented) {
            internalOnClick(event);
        }
    };

    return (
        <Button
            {...props}
            href={external ? href : linkHref}
            target={target}
            onClick={(external ? onClick : handleClick) as ButtonProps["onClick"]}
        />
    );
}
