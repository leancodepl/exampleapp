import { returnToParameterName } from "@leancodepl/kratos";
import { Outlet, useLocation } from "react-router";
import { useIsLoggedIn } from "../../../_hooks/useIsLoggedIn";
import { loginRoute } from "../../../publicRoutes";
import { Redirect } from "../../common/Redirect";

export default function LoggedInGuard() {
    const isLoggedIn = useIsLoggedIn();
    const { pathname, search } = useLocation();

    if (isLoggedIn === false) {
        const params = new URLSearchParams({ [returnToParameterName]: `${pathname}${search}` });

        const redirectUrl = `${loginRoute}?${params}`;

        return <Redirect path={redirectUrl} />;
    }

    if (isLoggedIn === undefined) {
        return null;
    }

    return <Outlet />;
}
