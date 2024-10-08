import React, { ChangeEvent, MouseEvent, useState } from 'react'

// Our stuff
import { useHttpRequestService } from '../../contexts/HttpRequestServiceContext';
import UriBuilder from '../../utils/UriBuilder';
import Title from '../../components/title/Title';
import { getRegisterConfig, IUserRegister } from '../../configs/form-config';
import InputForm from '../../components/input-form/InputForm';
import Loader from '../../components/loader/Loader';

const Register = (): JSX.Element => {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [userRegister, setUserRegister] = useState<IUserRegister>({ userName: '', email: '', password: '' });
    const httpRequestService = useHttpRequestService();

    const handleRegister = async (event: MouseEvent<HTMLInputElement>) => {
        event.preventDefault();
        setIsLoading(true);

        const registerUri = UriBuilder.use().Register().Build();
        try {
            const response = await httpRequestService.PostResourceAsync<IUserRegister>(registerUri, userRegister);
            console.log('Response value:', response);
        }
        catch (error) {
            console.log('Error fetching data:', error);
        }
        finally {
            setIsLoading(false);
        }
    }

    const handleInputChange = (event: ChangeEvent<HTMLInputElement>): void => {
        const { name, value } = event.target;

        setUserRegister((prevState) => ({
            ...prevState,
            [name]: value
        }));
    };

    const content =
        (
            <>
                <Title title={'Register'} />
                <br />
                <InputForm configurations={getRegisterConfig({ onChange: handleInputChange, onClick: handleRegister, formDetails: userRegister })} />
            </>
        )

    return (
        <div className='main-container'>
            {isLoading ? <Loader /> : content}
        </div>
    )
}

export default Register