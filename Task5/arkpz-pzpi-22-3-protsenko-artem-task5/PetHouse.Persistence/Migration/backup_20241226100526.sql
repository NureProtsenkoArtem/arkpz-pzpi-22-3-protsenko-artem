PGDMP          
            |         
   pethousedb    16.3    16.3                0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false                       0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false                       0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false                       1262    33245 
   pethousedb    DATABASE     �   CREATE DATABASE pethousedb WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Ukrainian_Ukraine.1251';
    DROP DATABASE pethousedb;
                postgres    false            �            1259    41058    Devices    TABLE       CREATE TABLE public."Devices" (
    "DeviceId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Model" text NOT NULL,
    "DeviceStatus" integer NOT NULL,
    "FeedingMode" integer NOT NULL,
    "RecognitionEnabled" boolean NOT NULL,
    "CameraEnabled" boolean NOT NULL
);
    DROP TABLE public."Devices";
       public         heap    postgres    false            �            1259    41063    HealthAnalyses    TABLE     m  CREATE TABLE public."HealthAnalyses" (
    "HealthAnalysisId" uuid NOT NULL,
    "PetId" uuid NOT NULL,
    "AnalysisDate" timestamp with time zone NOT NULL,
    "AnalysisStartDate" date NOT NULL,
    "AnalysisEndDate" date NOT NULL,
    "CaloriesConsumed" double precision NOT NULL,
    "HealthAnalysisType" integer NOT NULL,
    "Recomendations" text NOT NULL
);
 $   DROP TABLE public."HealthAnalyses";
       public         heap    postgres    false            �            1259    41068    Meals    TABLE     �  CREATE TABLE public."Meals" (
    "MealId" uuid NOT NULL,
    "PetId" uuid NOT NULL,
    "PortionSize" double precision NOT NULL,
    "StartTime" timestamp with time zone NOT NULL,
    "CaloriesPerMeal" double precision NOT NULL,
    "CaloriesConsumed" double precision NOT NULL,
    "AdaptiveAdjustment" boolean NOT NULL,
    "FoodType" text NOT NULL,
    "CalorificValue" double precision NOT NULL,
    "MealStatus" integer NOT NULL,
    "IsDaily" boolean NOT NULL
);
    DROP TABLE public."Meals";
       public         heap    postgres    false            �            1259    41073    Pets    TABLE     7  CREATE TABLE public."Pets" (
    "PetId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "PetName" text NOT NULL,
    "PetType" text NOT NULL,
    "PetWeight" double precision NOT NULL,
    "CaloriesPerDay" double precision NOT NULL,
    "ActivityLevel" integer NOT NULL,
    "RecognizableData" text NOT NULL
);
    DROP TABLE public."Pets";
       public         heap    postgres    false            �            1259    41078    Users    TABLE     #  CREATE TABLE public."Users" (
    "UserId" uuid NOT NULL,
    "Name" text NOT NULL,
    "Email" text NOT NULL,
    "Password" text NOT NULL,
    "UserRole" integer NOT NULL,
    "VerificationCode" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    "IsVerified" boolean NOT NULL
);
    DROP TABLE public."Users";
       public         heap    postgres    false            �            1259    41083    __EFMigrationsHistory    TABLE     �   CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);
 +   DROP TABLE public."__EFMigrationsHistory";
       public         heap    postgres    false                      0    41058    Devices 
   TABLE DATA           �   COPY public."Devices" ("DeviceId", "UserId", "Model", "DeviceStatus", "FeedingMode", "RecognitionEnabled", "CameraEnabled") FROM stdin;
    public          postgres    false    215   j'                 0    41063    HealthAnalyses 
   TABLE DATA           �   COPY public."HealthAnalyses" ("HealthAnalysisId", "PetId", "AnalysisDate", "AnalysisStartDate", "AnalysisEndDate", "CaloriesConsumed", "HealthAnalysisType", "Recomendations") FROM stdin;
    public          postgres    false    216   �'                 0    41068    Meals 
   TABLE DATA           �   COPY public."Meals" ("MealId", "PetId", "PortionSize", "StartTime", "CaloriesPerMeal", "CaloriesConsumed", "AdaptiveAdjustment", "FoodType", "CalorificValue", "MealStatus", "IsDaily") FROM stdin;
    public          postgres    false    217   �(       	          0    41073    Pets 
   TABLE DATA           �   COPY public."Pets" ("PetId", "UserId", "PetName", "PetType", "PetWeight", "CaloriesPerDay", "ActivityLevel", "RecognizableData") FROM stdin;
    public          postgres    false    218   �*       
          0    41078    Users 
   TABLE DATA           �   COPY public."Users" ("UserId", "Name", "Email", "Password", "UserRole", "VerificationCode", "CreatedAt", "IsVerified") FROM stdin;
    public          postgres    false    219   �+                 0    41083    __EFMigrationsHistory 
   TABLE DATA           R   COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
    public          postgres    false    220   �,       e           2606    41087    Devices PK_Devices 
   CONSTRAINT     \   ALTER TABLE ONLY public."Devices"
    ADD CONSTRAINT "PK_Devices" PRIMARY KEY ("DeviceId");
 @   ALTER TABLE ONLY public."Devices" DROP CONSTRAINT "PK_Devices";
       public            postgres    false    215            h           2606    41089     HealthAnalyses PK_HealthAnalyses 
   CONSTRAINT     r   ALTER TABLE ONLY public."HealthAnalyses"
    ADD CONSTRAINT "PK_HealthAnalyses" PRIMARY KEY ("HealthAnalysisId");
 N   ALTER TABLE ONLY public."HealthAnalyses" DROP CONSTRAINT "PK_HealthAnalyses";
       public            postgres    false    216            k           2606    41091    Meals PK_Meals 
   CONSTRAINT     V   ALTER TABLE ONLY public."Meals"
    ADD CONSTRAINT "PK_Meals" PRIMARY KEY ("MealId");
 <   ALTER TABLE ONLY public."Meals" DROP CONSTRAINT "PK_Meals";
       public            postgres    false    217            n           2606    41093    Pets PK_Pets 
   CONSTRAINT     S   ALTER TABLE ONLY public."Pets"
    ADD CONSTRAINT "PK_Pets" PRIMARY KEY ("PetId");
 :   ALTER TABLE ONLY public."Pets" DROP CONSTRAINT "PK_Pets";
       public            postgres    false    218            p           2606    41095    Users PK_Users 
   CONSTRAINT     V   ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("UserId");
 <   ALTER TABLE ONLY public."Users" DROP CONSTRAINT "PK_Users";
       public            postgres    false    219            r           2606    41097 .   __EFMigrationsHistory PK___EFMigrationsHistory 
   CONSTRAINT     {   ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");
 \   ALTER TABLE ONLY public."__EFMigrationsHistory" DROP CONSTRAINT "PK___EFMigrationsHistory";
       public            postgres    false    220            c           1259    41098    IX_Devices_UserId    INDEX     M   CREATE INDEX "IX_Devices_UserId" ON public."Devices" USING btree ("UserId");
 '   DROP INDEX public."IX_Devices_UserId";
       public            postgres    false    215            f           1259    41099    IX_HealthAnalyses_PetId    INDEX     Y   CREATE INDEX "IX_HealthAnalyses_PetId" ON public."HealthAnalyses" USING btree ("PetId");
 -   DROP INDEX public."IX_HealthAnalyses_PetId";
       public            postgres    false    216            i           1259    41100    IX_Meals_PetId    INDEX     G   CREATE INDEX "IX_Meals_PetId" ON public."Meals" USING btree ("PetId");
 $   DROP INDEX public."IX_Meals_PetId";
       public            postgres    false    217            l           1259    41101    IX_Pets_UserId    INDEX     G   CREATE INDEX "IX_Pets_UserId" ON public."Pets" USING btree ("UserId");
 $   DROP INDEX public."IX_Pets_UserId";
       public            postgres    false    218            s           2606    41102    Devices FK_Devices_Users_UserId    FK CONSTRAINT     �   ALTER TABLE ONLY public."Devices"
    ADD CONSTRAINT "FK_Devices_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("UserId") ON DELETE CASCADE;
 M   ALTER TABLE ONLY public."Devices" DROP CONSTRAINT "FK_Devices_Users_UserId";
       public          postgres    false    219    4720    215            t           2606    41107 +   HealthAnalyses FK_HealthAnalyses_Pets_PetId    FK CONSTRAINT     �   ALTER TABLE ONLY public."HealthAnalyses"
    ADD CONSTRAINT "FK_HealthAnalyses_Pets_PetId" FOREIGN KEY ("PetId") REFERENCES public."Pets"("PetId") ON DELETE CASCADE;
 Y   ALTER TABLE ONLY public."HealthAnalyses" DROP CONSTRAINT "FK_HealthAnalyses_Pets_PetId";
       public          postgres    false    218    216    4718            u           2606    41112    Meals FK_Meals_Pets_PetId    FK CONSTRAINT     �   ALTER TABLE ONLY public."Meals"
    ADD CONSTRAINT "FK_Meals_Pets_PetId" FOREIGN KEY ("PetId") REFERENCES public."Pets"("PetId") ON DELETE CASCADE;
 G   ALTER TABLE ONLY public."Meals" DROP CONSTRAINT "FK_Meals_Pets_PetId";
       public          postgres    false    4718    218    217            v           2606    41117    Pets FK_Pets_Users_UserId    FK CONSTRAINT     �   ALTER TABLE ONLY public."Pets"
    ADD CONSTRAINT "FK_Pets_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("UserId") ON DELETE CASCADE;
 G   ALTER TABLE ONLY public."Pets" DROP CONSTRAINT "FK_Pets_Users_UserId";
       public          postgres    false    218    219    4720               `   x��1�0 ����>�����~`+gl�������̱�4�5�&׺��AoV����'�!k��T������s~���&̲-���L         �   x�=��n�0 g�+�L=�|F�1�dR��V6,u���]���ˮ[���s^��Pe|���Q܂�ɧ]P�,+g8���_%�)��Lf1N�Q �f��E��ż�o�zBM�Z-=F���>��ʀt���m{�iC�]����E�k{¨?�!5�c?G��+w=?�<�/$-=%         �  x���;n�1��O�>��/I�%%JA �Eb�?�H�4V�j>�G�ݩ��I��.�J`���+�� ��L�3��=�������E�.FV ���؃�ž _��݊�B������~�|����ۥ��~�p�5)
�Rܛ��s��6������b^�z�x}��L[�DT��RSh�Ӄ4�ܣ��N��_�Zlw��#�_�WO�����p0l��a=�h�M	���q{3;X!�e%�h������k���2;8o��;�jS��FΒ�*uU0lTk� Wpʎ"���(�,�wY0�lw� ;����3F9�a<��UU10�nq��>o5�B�Jk8�M}!:��(:Sd.zS�W�P�Q�5��7�Fh8l� ��޺�n��i�yil�3���j���o�c��O�l{~W9��7���hFk߾)5\��b@_i�ƭ��O�ԧi��o��/�*      	   "  x���An�0E���� ���� �U��J��cj6��ӗ�ٗE������=}�-���O���C�
{r����&�l%��i@o�D���6�!����S�/~=�|T�e�ںVZ9_RWU���yMѯ�;�2�2�0�K K>�u��'�a>WK�q��O�K8s%;p+�;�N�?��2�l���Zq�B�@������l�iN�E������f���6;�]���ak��D& �hp�
x!v�Yv�S~�Y�a�iU�hۯy���
�#���r��,���	���      
     x�U�MO�0 �s�+8p3�ۮ�v���B5�˻���X���{5ƃ����iȤ�)��J���B�:̭��h�v���H�Z7�X^/�������(�?��,I>/˛[����U$Yެ[��-�¸�n�4��"����'@k$�@����GL�\s��y*c�h5�I*S�)� �)h -U�YJ&��%������V�Z���{�������˗a���NMl^�ي�<�0߬��hY?��ȗ�)AS�ͤ!��R8�����`����{��5[�         *   x�32021424124451����,�L���3�3������ ���     