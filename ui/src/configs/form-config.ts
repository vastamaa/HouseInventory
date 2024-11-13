import { ChangeEvent } from "react";

export interface ILoginField {
    label: string,
    name: string,
    type: string,
    defaultText: string,
    onChange: (event: any) => void; 
    value: string;
}

export interface IUserLogin {
    email: string;
    password: string;
    rememberMe?: boolean | null;
  }

interface IGetLoginConfigProps {
    onChange: (event: ChangeEvent<HTMLInputElement>) => void;
    formDetails: IUserLogin;
}

export const getLoginConfig = (props: IGetLoginConfigProps): ILoginField[] => {
    const { onChange, formDetails } = props;

    return [
        {
            label: 'E-mail',
            name: 'email',
            type: 'email',
            defaultText: 'Enter your e-mail here',
            onChange: event => onChange(event),
            value: formDetails.email
        },
        {
            label: 'Password',
            name: 'password',
            type: 'password',
            defaultText: 'Enter your password here',
            onChange: event => onChange(event),
            value: formDetails.password
        }
    ]
}
