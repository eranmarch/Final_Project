----------------------------------------------------------------
-- Orbotech Ltd. 
-- PCB Division, AOI Department 
-- System(s)      : FUSION
-- Card           : "MVC_2"  
-- Name           : mapping_package.vhd 
-- Author         : Danny Shalom. 
-- Entity description:

-- Version history:
-- Version 1, by Danny Shalom, 1/1/2008 - Initial version.
----------------------------------------------------------------
--

library ieee;
use ieee.std_logic_1164.all;
use ieee.std_logic_arith.all;
use ieee.std_logic_unsigned.all;
use work.global_package_top.all ;
use work.global_package.all ;
use work.FPGA_DATE.all;


package mapping_package   is   

-- ****************************************************************************
constant    software_version : Integer := 256; 				--1*256 + 0 ;       -- version 1.0
constant 	fpga_time_reg     : integer := conv_integer(FPGA_HOUR & FPGA_MINUTE);
constant 	fpga_date_reg     : integer := conv_integer(FPGA_DAY & FPGA_MONTH & FPGA_YEAR);
-- ****************************************************************************

-- ****************************************************************************
-- MAPPING OF AVALON ADDRESS
-- ****************************************************************************
type        avalon_map_defenition  is  (
		 -- 
				reg1,
				reg2,
		 -- group1
				reg3,
                last_deff) ;    -- last_deff must be the last in the list.


constant  number_of_fields  : integer := avalon_map_defenition'pos(last_deff) ;

type        RW_type         is (RD,     -- for read port
                                WR,     -- for write port
                                RD_WR,  -- for read and write port
                                FIELD); -- for field definition in previous port

type        fpga_type       is (A,B,C,D,E,G,AG,ABC,ABCG) ;
                            -- A for RED channel, B for BLUE channel, C for GREEN channel, D for Driver, G for GLOBAL

type        field_type          is record
                            name    : avalon_map_defenition ;
                            address : integer range 0 to 2**avalon_addr_bits-1 ;
                            MAIS    : integer range 0 to 4 ; -- Memory Address Increment Step
                            lsb     : integer ;
                            msb     : integer ;
                            f_type  : RW_type ;
                            fpga    : fpga_type ;
                            init    : integer ;             -- value after reset
                            end record ;


type        fields_table_type   is array(1 to number_of_fields) of field_type ;

-- Software address is [FPGA_offset + ADDRESS * 8]
-- FPGA_offset = 0 for D, 8192 for G, 16384 for A, 24576 for B, 32768 for C, 40960 for E

constant    avalon_fields_table     : fields_table_type :=  (
--               field name or port name                ,ADDRESS,MAIS,LSB,MSB, TYPE , FPGA,INIT
		 -- 
				(reg1                                   ,       1,  0,  0, 31, RD   , G   ,   0),
				(reg2                                   ,       2,  0,  0, 31, RD   , G   ,   0),
		 -- group1
				(reg3                                   ,       3,  0,  0, 31, RD   , G   ,   0)
                );

function    address_of      (port_name  : avalon_map_defenition) return Integer ;
function    lsb_of          (field_name : avalon_map_defenition) return Integer ;
function    msb_of          (field_name : avalon_map_defenition) return Integer ;
function    init_of         (port_name  : avalon_map_defenition) return Std_logic ;
function    init_of         (port_name  : avalon_map_defenition) return Std_logic_Vector ;
function    memory_inc_step (address : Std_Logic_Vector) return Integer ;
function    port_exist      (address : Integer) return boolean ;


end mapping_package;


package body mapping_package is


function    address_of  (port_name : avalon_map_defenition) return Integer is
    variable    address : Integer ;
    begin
        address := 0 ;
        for i in 1 to number_of_fields loop
            if port_name = avalon_fields_table(i).name then
                address := avalon_fields_table(i).address ;
                exit ;
            end if ;
        end loop ;
        return address ;
    end function ;


function    lsb_of  (field_name : avalon_map_defenition) return Integer is
    variable    lsb : Integer ;
    begin
        lsb := 0 ;
        for i in 1 to number_of_fields loop
            if field_name = avalon_fields_table(i).name then
                lsb := avalon_fields_table(i).lsb ;
                exit ;
            end if ;
        end loop ;
        return lsb ;
    end function ;


function    msb_of  (field_name : avalon_map_defenition) return Integer is
    variable    msb : Integer ;
    begin
        msb := 0 ;
        for i in 1 to number_of_fields loop
            if field_name = avalon_fields_table(i).name then
                msb := avalon_fields_table(i).msb ;
                exit ;
            end if ;
        end loop ;
        return msb ;
    end function ;


function    init_of (port_name : avalon_map_defenition) return Std_Logic is
    variable    init : Std_Logic ;
    begin
        init := '0' ;
        for i in 1 to number_of_fields loop
            if port_name = avalon_fields_table(i).name then
                if avalon_fields_table(i).init > 0 then
                    init := '1' ;
                end if ;
                exit ;
            end if ;
        end loop ;
        return init ;
    end function ;


function    init_of (port_name : avalon_map_defenition) return Std_Logic_Vector is
    variable    init : Std_Logic_Vector(31 downto 0) ;
    begin
        init := (others => '0') ;
        for i in 1 to number_of_fields loop
            if port_name = avalon_fields_table(i).name then
                init := conv_std_logic_vector(avalon_fields_table(i).init,32) ;
                return init(avalon_fields_table(i).msb - avalon_fields_table(i).lsb downto 0) ;
                exit ;
            end if ;
        end loop ;
    end function ;


function    memory_inc_step (address : Std_Logic_Vector) return Integer is
    variable    MAIS : Integer ;
    begin
        MAIS := 0 ;
        for i in 1 to number_of_fields loop
            if address = avalon_fields_table(i).address and avalon_fields_table(i).f_type /= FIELD then
                MAIS := avalon_fields_table(i).MAIS ;
                exit ;
            end if ;
        end loop ;
        return MAIS ;
    end function ;


function    port_exist  (address : Integer) return boolean is
    variable    exist : boolean ;
    begin
      exist := false ;
        for i in 1 to number_of_fields loop
            if address = (avalon_fields_table(i).address mod (2**real_address_bits)) and avalon_fields_table(i).f_type /= FIELD then
                exist := true ;
                exit ;
            end if ;
        end loop ;
        return exist ;
    end function ;
        

end mapping_package;
