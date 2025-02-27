import { UserProvider } from "../providers";

type Props = {
    children: React.ReactNode
}
export const Providers: React.FC<Props> = (props) => {
    return (
        <UserProvider>
            {props.children}
        </UserProvider>
    );
}