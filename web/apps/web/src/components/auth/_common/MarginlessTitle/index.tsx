import { ReactNode } from "react";
import { MarginlessTitle } from "./styles";

type CardTitleProps = { children?: ReactNode };

export function CardTitle(props: CardTitleProps) {
    return <MarginlessTitle {...props} level={4} />;
}
