import { Outlet } from "react-router";
import { useIsLoggedIn } from "../../../_hooks/useIsLoggedIn";
import { Redirect } from "../../common/Redirect";

export default function NotLoggedInGuard() {
    const isLoggedIn = useIsLoggedIn();

    if (isLoggedIn === true) {
        return <Redirect path="/" />;
    }

    if (isLoggedIn === undefined) {
        return null;
    }

    return <Outlet />;
}
