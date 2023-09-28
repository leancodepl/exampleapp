import { useRoutes } from "react-router";
import { routes } from "../routes";

export default function AuthenticatedApp() {
    const router = useRoutes(routes);

    return <main>{router}</main>;
}
