import { useRoutes } from "react-router";
import { AppLayout } from "../components/AppLayout";
import { routes } from "../routes";

export default function AuthenticatedApp() {
    const router = useRoutes(routes);

    return <AppLayout>{router}</AppLayout>;
}
